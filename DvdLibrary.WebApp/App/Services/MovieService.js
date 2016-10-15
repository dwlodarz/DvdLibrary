(function () {
    'use strict';
    angular.module('app.services', [])
        .service("movieService", ['$q', '$http', function ($q, $http) {
            var apiUri = 'http://localhost/DvdLibrary.WebApi/api/movie/';

            function getMovies(query) {
                var getMoviesRequest = $http({
                    method: 'GET',
                    url: apiUri + 'AvailableMovies?q=' + query
                }).then(function (response) {
                    if (response && response.data) {
                        return response.data;
                    }
                });
                return getMoviesRequest;
            }

            //function addNewEvent(event) {
            //    var addEventRequest = $http({
            //        method: "POST",
            //        url: apiUri + 'event?eventGuid=' + event.guid,
            //        dataType: 'json',
            //        headers: {
            //            "Content-Type": "application/json"
            //        },
            //        data: event
            //    }).then(function (response) {
            //        if (response && response.data) {
            //            event = convertToCalendarEvent(response.data)
            //            storedEvents.push(event);
            //            return storedEvents;
            //        }
            //    });
            //    return addEventRequest;
            //}

            //function deleteEvent(event) {
            //    var deleteEventRequest = $http({
            //        method: "DELETE",
            //        url: apiUri + 'event?eventGuid=' + event.guid,
            //        dataType: 'json',
            //        headers: {
            //            "Content-Type": "application/json"
            //        },
            //        data: event
            //    }).then(function (response) {
            //        if (response && response.data) {
            //            storedEvents = _.reject(storedEvents, function (item) { return item.guid == event.guid; });
            //        }
            //        return storedEvents;
            //    });
            //    return deleteEventRequest;
            //}


            return {
                GetMovies: getMovies,
                //AddNewEventEntry: addNewEvent,
                //DeleteEventEntry: deleteEvent,
                //EditEvent: editEvent
            };
        }]);
})();