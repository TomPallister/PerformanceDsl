using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Newtonsoft.Json;
using PerformanceDsl.Logging;

namespace PerformanceDsl
{
    public class PerformanceServer
    {
        /// <summary>
        ///     This method kicks off the process of the server sending out the tests
        /// </summary>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        public async Task BeginTestRun(TestSuite testSuite)
        {
            //first thing we do is make sure all test runs have ips, if they dont we set them
            //up in AWS, this is very tighly coupled at the moment. we get the instance ids back so we 
            //can close them later
            Log4NetLogger.LogEntry(GetType(), "BeginTestRun", "starting test run", LoggerLevel.Info);
            var ec2Client = new AmazonEC2Client(RegionEndpoint.EUWest1);
            Log4NetLogger.LogEntry(GetType(), "BeginTestRun", "created EC2 client, now assiging ips", LoggerLevel.Info);
            List<string> instanceIds = AssignAgentIps(ec2Client, testSuite);
            Log4NetLogger.LogEntry(GetType(), "BeginTestRun", "assigned ips if there were any required, about to run tests", LoggerLevel.Info);
            //now we need to send our dlls to the agents
            UploadDllsToAgents(testSuite);


            //once we are all good we run the tests. 
            await Run(testSuite);
            Log4NetLogger.LogEntry(GetType(), "BeginTestRun", "tests have finished running, now terminate any agents", LoggerLevel.Info);
            //we return null from AssignAgentIps if we dont need any IPs, bad programming but it will do 
            //for now.
            if (instanceIds != null)
            {
                Log4NetLogger.LogEntry(GetType(), "BeginTestRun", "terminating agents", LoggerLevel.Info);
                //terminate the agents LIKE A BAUSSSSSSSSSSS
                TerminateAgents(ec2Client, instanceIds);
                Log4NetLogger.LogEntry(GetType(), "BeginTestRun", "terminated agents", LoggerLevel.Info);
            }
        }

        public void UploadDllsToAgents(TestSuite testSuite)
        {
            if (testSuite.DllsThatNeedUploadingToAgent.Count > 0)
            {
                var agentIps = testSuite.Tests.Select(x => x.Agent);
                foreach (var agentIp in agentIps)
                {
                    foreach (var dll in testSuite.DllsThatNeedUploadingToAgent)
                    {
                        var fileName = GetFileName(dll);

                        UploadFile(fileName, string.Format("http://{0}:9999", agentIp), dll);
                    }
                }
            }
        }

        private void UploadFile(string fileName, string url, string filePath)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("FileName", fileName);
                client.UploadFile(new Uri(url), filePath);
            }
        }

        private string GetFileName(string filePath)
        {
            var lastIndex = filePath.LastIndexOf("\\", StringComparison.Ordinal);
            var fileName = filePath.Substring(lastIndex + 1);
            return fileName;
        }

        private async Task Run(TestSuite testSuite)
        {
            Log4NetLogger.LogEntry(GetType(), "Run", "starting to send tests to agents", LoggerLevel.Info);
            var tasks = new Task[testSuite.Tests.Count];
            for (int i = 0; i < testSuite.Tests.Count; i++)
            {
                int copy = i;
                Task task = Task.Run(() => PostToAgent(testSuite.Tests[copy].Agent, testSuite.Tests[copy].TestRun));
                tasks[i] = task;
            }
            Log4NetLogger.LogEntry(GetType(), "Run", "finished sending tests to agents", LoggerLevel.Info);
            //wait for all the tasks to finish
            await Task.WhenAll(tasks);
            Log4NetLogger.LogEntry(GetType(), "Run", "all tasks finished returning", LoggerLevel.Info);

        }

        private void TerminateAgents(AmazonEC2Client ec2Client, List<string> instanceIds)
        {
            var termRequest = new TerminateInstancesRequest {InstanceIds = instanceIds};
            TerminateInstancesResponse termResponse = ec2Client.TerminateInstances(termRequest);
            if (termResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Agents were not terminated succesfully");
            }
        }

        private async Task PostToAgent(string agent, TestRun testRun)
        {
            Log4NetLogger.LogEntry(GetType(), "PostToAgent", "posting testrun to agent", LoggerLevel.Info);
            string testRunJson = JsonConvert.SerializeObject(testRun);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(testRunJson);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage result =
                    await httpClient.PostAsync(string.Format("http://{0}:9999", agent), content);
                result.EnsureSuccessStatusCode();
            }
            Log4NetLogger.LogEntry(GetType(), "PostToAgent", "finished posting testrun to agent", LoggerLevel.Info);
        }

        public List<string> AssignAgentIps(AmazonEC2Client ec2Client, TestSuite testSuite)
        {
            int requiredInstances = testSuite.Tests.Count(testToGiveIp => string.IsNullOrWhiteSpace(testToGiveIp.Agent));

            if (requiredInstances > 0)
            {
                var runInstancesRequest = new RunInstancesRequest
                {
                    ImageId = "ami-df844ba8",
                    InstanceType = "t1.micro",
                    MinCount = requiredInstances,
                    MaxCount = requiredInstances,
                    KeyName = "Fourth"
                };
                runInstancesRequest.SecurityGroups.Add("Controller");
                RunInstancesResponse runResponse = ec2Client.RunInstances(runInstancesRequest);
                List<Instance> instances = runResponse.Reservation.Instances;
                List<string> instanceIDs = instances.Select(item => item.InstanceId).ToList();
                WaitForInstancesToBeRunning(ec2Client, instanceIDs);
                var instancesRequest = new DescribeInstancesRequest {InstanceIds = instanceIDs};
                DescribeInstancesResponse statusResponse = ec2Client.DescribeInstances(instancesRequest);
                List<string> ipAddresses =
                    statusResponse.Reservations[0].Instances.Select(x => x.PublicIpAddress).ToList();
                //we now have our running instances and we need to assign the ips to our tests
                foreach (Test test in testSuite.Tests.Where(test => string.IsNullOrWhiteSpace(test.Agent)))
                {
                    //assign the first free Id
                    test.Agent = ipAddresses.First();
                    //then remove it from the list
                    ipAddresses.RemoveAt(0);
                }

                //now we need to make sure all instances are ready
                MakeSureAgentsCanBeUsed(ec2Client, instanceIDs);
                return instanceIDs;
            }
            return null;
        }

        private void MakeSureAgentsCanBeUsed(AmazonEC2Client ec2Client, List<string> instanceIds)
        {
            List<IsAgentReady> agentsThatAreReady =
                instanceIds.Select(instanceId => new IsAgentReady {InstanceID = instanceId, IsReady = false}).ToList();
            while (agentsThatAreReady.Any(x => x.IsReady == false))
            {
                foreach (string instanceId in instanceIds)
                {
                    bool already = agentsThatAreReady.Any(x => x.InstanceID == instanceId && x.IsReady);
                    if (already) continue;
                    var resquest = new DescribeInstanceStatusRequest
                    {
                        InstanceIds = new List<string>
                        {
                            instanceId
                        }
                    };
                    DescribeInstanceStatusResponse response = ec2Client.DescribeInstanceStatus(resquest);
                    if (response.InstanceStatuses.Count > 0 &&
                        response.InstanceStatuses[0].SystemStatus.Status == SummaryStatus.Ok &&
                        response.InstanceStatuses[0].Status.Status == SummaryStatus.Ok)
                    {
                        IsAgentReady agent = agentsThatAreReady.First(x => x.InstanceID == instanceId);
                        agent.IsReady = true;
                    }
                }
            }
        }

        /// <summary>
        ///     This method waits for all instances passed in to be marked as running in AWS. I assume
        ///     we can start using them once they are running :)
        /// </summary>
        /// <param name="ec2Client"></param>
        /// <param name="instanceIds"></param>
        private void WaitForInstancesToBeRunning(AmazonEC2Client ec2Client, List<string> instanceIds)
        {
            List<IsAgentReady> agentsThatAreReady =
                instanceIds.Select(instanceId => new IsAgentReady {InstanceID = instanceId, IsReady = false}).ToList();

            while (agentsThatAreReady.Any(x => x.IsReady == false))
            {
                foreach (string instanceId in instanceIds)
                {
                    //first check this isnt already ready
                    bool already = agentsThatAreReady.Any(x => x.InstanceID == instanceId && x.IsReady);

                    if (already) continue;
                    //get its status
                    var describeInstanceStatusRequest = new DescribeInstanceStatusRequest
                    {
                        InstanceIds = new List<string>
                        {
                            instanceId
                        }
                    };
                    DescribeInstanceStatusResponse response =
                        ec2Client.DescribeInstanceStatus(describeInstanceStatusRequest);
                    if (response.InstanceStatuses.Count > 0 &&
                        response.InstanceStatuses[0].InstanceState.Name == InstanceStateName.Running)
                    {
                        IsAgentReady agent = agentsThatAreReady.First(x => x.InstanceID == instanceId);
                        agent.IsReady = true;
                    }
                }
            }
        }
    }

    public class IsAgentReady
    {
        public string InstanceID { get; set; }
        public bool IsReady { get; set; }
    }
}