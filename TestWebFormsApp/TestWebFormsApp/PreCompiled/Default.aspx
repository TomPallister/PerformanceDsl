<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestWebFormsApp._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Fourth Technical QA Test</h2>
            </hgroup>
            <p>
                This website is for the Fourth Technical QA Test
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
<h3>You will need to automate the exercis using the following:</h3>
<ul>
    <li>SpecFlow (Gherkin Syntax)</li>
    <li>Selenium WebDriver</li>
    <li>Page Object Model</li>
    <li>C# and NUnit</li>
    <li>Visual Studio</li>
</ul>

<ol class="round">
    <li class="one">
        <h5>Exercise</h5>
        This exercise will assess your ability to create an automated test using SpecFlow, Selenium WebDriver, Page Object Model and C#
        
        <h6>Exercise Requirements</h6>
        <ol>
            <li>Start <a href=/Account/Register><strong>Exercise 1</strong></a></li> 
            <li>Register <strong>2</strong> new user accounts using <strong>1</strong> SpecFlow feature file and scenario.</li>
            <li>Verify the user is logged in.</li>
        </ol>
    </li>
</ol>

</asp:Content>