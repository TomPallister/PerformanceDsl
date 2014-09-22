App = Ember.Application.create({
    // Basic logging, e.g. "Transitioned into 'post'"
  LOG_TRANSITIONS: true, 

  // Extremely detailed logging, highlighting every internal
  // step made while transitioning into a route, including
  // `beforeModel`, `model`, and `afterModel` hooks, and
  // information about redirects and aborted transitions
  LOG_TRANSITIONS_INTERNAL: true,
  LOG_ACTIVE_GENERATION: true
});

App.Router.map(function() {
    this.resource('index', { path: '/' });
    this.resource('problem', { path: '/problem/:errorMessage' });
    this.resource('base', function(){
      this.resource('runs', { path: '/runs/'});
      this.resource('run', { path: '/run/:testRunGuid'});
    });
});

App.RunsView = Ember.View.extend({
  didInsertElement: function() {
  }
});

App.RunsRoute = Ember.Route.extend({  
  beforeModel: function(data) {
  },
  setupController: function(controller, data) {

    var jqxhr = $.ajax({
        type: "GET",
        crossDomain: true,
        contentType: "application/json; charset=utf-8",
        url: App.GetApiUrl() + "/api/run",
        dataType: 'json',
        processData: false,
        headers: {
           "X-Custom-Header": "PerformanceTestResults",
           "Authorization" : localStorage.getItem("Token")
         }
    }).done(function(response){
        App.FormatDate(response);
        controller.set('model', response);
      }).fail(function(response){
        controller.transitionToRoute('problem', response.responseText);
      });
  }
});

App.RunsController = Ember.ArrayController.extend({
    init: function(){
      this._super();
    }
});

App.RunView = Ember.View.extend({
  didInsertElement: function() {
  }
});

App.RunRoute = Ember.Route.extend({  
  beforeModel: function(data) {
  },
  setupController: function(controller, data) {

    var jqxhr = $.ajax({
        type: "GET",
        crossDomain: true,
        contentType: "application/json; charset=utf-8",
        url: App.GetApiUrl() + "/api/run/" + data.testRunGuid,
        dataType: 'json',
        processData: false,
        headers: {
           "X-Custom-Header": "PerformanceTestResults",
           "Authorization" : localStorage.getItem("Token")
         }
    }).done(function(response){
        controller.set('model', response.RowSummaries);
      }).fail(function(response){
        controller.transitionToRoute('problem', response.responseText);
      });
  }
});

App.RunController = Ember.ArrayController.extend({
    init: function(){
      this._super();
    }
});

App.FormatDate = function(arrayOfRuns){
  for (var i = arrayOfRuns.length - 1; i >= 0; i--) {
    var day = moment(arrayOfRuns[i].StartDate);  
    arrayOfRuns[i].StartDate = day.format("DD-MM-YYYY HH:mm");
  };
}

App.IndexView = Ember.View.extend({

    didInsertElement: function() {
        var self = this;
        $("#password").keypress(function(e) {
          App.UpdateScroll(100);
        }).click(function(e) {
          App.UpdateScroll(100);
        });

        $("#username").keypress(function(e) {
          App.UpdateScroll(100);

        }).click(function(e) {
          App.UpdateScroll(100);
        });
      },

      willDestroyElement: function(){
        var removeHeight = this.scrollUpdated * 200
        var currentHeight = $('.container').height();
        //increase it by 200
        var newHeight = currentHeight - removeHeight - 200
        $('.container').css('height', newHeight);
      }
})

App.IndexRoute = Ember.Route.extend({
  setupController: function(controller) {
      }
});

App.IndexController = Ember.Controller.extend({
  loginFailed: false,
  isProcessing: false,
  isSlowConnection: false,
  timeout: null,
  login: function(username, password) {
    this.setProperties({
      loginFailed: false,
      isProcessing: true
    });
    this.set("timeout", setTimeout(this.slowConnection.bind(this), 5000));
    if(username != undefined && password != undefined){
      this.set("username", username);
      this.set("password", password);
    }
    var data = {
      UserName : this.get("username"),
      Password : this.get("password")
    }
    var jqxhr = $.ajax({
          type: "POST",
          contentType: "application/json; charset=utf-8",
          url: App.GetApiUrl() + "/api/auth",
          data: JSON.stringify(data),
          dataType: 'json',
          processData: false,
          headers: {
               "X-Custom-Header": "PerformanceTestResults",
         }
    });
    jqxhr.done(this.success.bind(this)).fail(this.failure.bind(this)).always(function(response) {});
  },

  success: function(response) {
    localStorage.setItem('Token', response.token);
    this.reset();
    this.transitionToRoute('runs');
  },

  failure: function(response) {
    this.reset();
    this.set("password", "");
    this.set("loginFailed", true);
  },

  slowConnection: function() {
    this.set("isSlowConnection", true);
  },

  reset: function() {
    clearTimeout(this.get("timeout"));
    this.setProperties({
      isProcessing: false,
      isSlowConnection: false
    });
  }
});

App.UpdateScroll = function(amount){
  if(!amount){
    amount = 200;
  }
  if($('.container').height() < 500){
  //get current height
  var currentHeight = $('.container').height();
  //increase it by 200
  var newHeight = currentHeight + amount
  $('.container').css('height', newHeight);
  }
};


App.GetApiUrl = function(){
  return "http://www.testresultsstore.dev";
}