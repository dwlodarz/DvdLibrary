(function () {
    'use strict';
    angular.module('app.services', [])
        .service("eventService", ['$q', '$http', function ($q, $http) {
            var apiUri = 'http://localhost/Dashboard.WebApi/api/';
            var storedEvents = [];

            function getStoredEvent() {
                var getEventRequest = $http({
                    method: 'GET',
                    url: apiUri + 'event'
                }).then(function (response) {
                    if (response && response.data) {
                        storedEvents = [];
                        for (var i = 0; i < response.data.length; i++) {
                            storedEvents.push(convertToCalendarEvent(response.data[i]))
                        }
                        return storedEvents;
                    }
                });
                return getEventRequest;
            }

            function addNewEvent(event) {
                var addEventRequest = $http({
                    method: "POST",
                    url: apiUri + 'event?eventGuid=' + event.guid,
                    dataType: 'json',
                    headers: {
                        "Content-Type": "application/json"
                    },
                    data: event
                }).then(function (response) {
                    if (response && response.data) {
                        event = convertToCalendarEvent(response.data)
                        storedEvents.push(event);
                        return storedEvents;
                    }
                });
                return addEventRequest;
            }

            function deleteEvent(event) {
                var deleteEventRequest = $http({
                    method: "DELETE",
                    url: apiUri + 'event?eventGuid=' + event.guid,
                    dataType: 'json',
                    headers: {
                        "Content-Type": "application/json"
                    },
                    data: event
                }).then(function (response) {
                    if (response && response.data) {
                        storedEvents = _.reject(storedEvents, function (item) { return item.guid == event.guid; });
                    }
                    return storedEvents;
                });
                return deleteEventRequest;
            }

            function editEvent(event) {
                var editedEvent = _.findWhere(storedEvents, { guid: event.guid });
                if (editedEvent) {
                    //UpdateApi
                }
            }

            function convertToCalendarEvent(model) {
                return {
                    id: model.Id,
                    title: model.Title,
                    type: 'warning',
                    description: model.Description,
                    startsAt: moment.utc(model.StartsAt).toDate(),
                    endsAt: moment.utc(model.EndsAt).toDate(),
                    patient: model.patient,
                    guid: model.Guid,
                    draggable: true,
                    resizable: true,
                    editable: true
                };
            }

            return {
                GetStoredEvent: getStoredEvent,
                AddNewEventEntry: addNewEvent,
                DeleteEventEntry: deleteEvent,
                EditEvent: editEvent
            };
        }]);
})();