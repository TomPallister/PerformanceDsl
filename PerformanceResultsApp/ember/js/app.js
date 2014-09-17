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
    this.resource('run', { path: '/run/:testRunGuid' });
    this.resource('problem', { path: '/problem/:errorMessage' });
    this.resource('base', function(){
      this.resource('player', { path: '/player/:aggregateId'});
      this.resource('newchallenge', { path: '/player/new'});
    });
});

App.IndexView = Ember.View.extend({
  didInsertElement: function() {
  }
});

App.IndexRoute = Ember.Route.extend({  
  beforeModel: function(data) {
  },
  setupController: function(controller, data) {

    var jqxhr = $.ajax({
        type: "GET",
        crossDomain: true,
        contentType: "application/json; charset=utf-8",
        url: "http://www.testresultsstore.dev/api/run",
        dataType: 'json',
        processData: false,
    }).done(function(response){
        controller.set('model', JSON.parse(response));
      }).fail(function(response){
        controller.transitionToRoute('problem', response.responseText);
      });
  }
});

App.IndexController = Ember.ArrayController.extend({
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
        url: "http://www.testresultsstore.dev/api/run/" + data.testRunGuid,
        dataType: 'json',
        processData: false,
    }).done(function(response){
        controller.set('model', JSON.parse(response));
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





