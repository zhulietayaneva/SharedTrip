using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Data.Models;
using SharedTrip.Models.Trip;
using SharedTrip.Services;
using System.Linq;

using System;
using System.Globalization;

namespace SharedTrip.Controllers
{
    public class TripsController:Controller
    {
        private readonly ApplicationDbContext data;
        private readonly IValidator validator;

        public TripsController(
            ApplicationDbContext data,
            IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }
        [Authorize]
        public HttpResponse All()
        {
            if (!this.User.IsAuthenticated)
            {
                return Redirect("/Users/Login");
            }


            var tripsQuery = this.data
                .Trips
                .AsQueryable();

           
            var trips = tripsQuery
                .Select(t => new TripsAllFormModel
                {
                    Id=t.Id,
                    StartPoint=t.StartPoint,
                    EndPoint=t.EndPoint,
                    DepartureTime=t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                    Seats=t.Seats,
                    
                })
                .ToList();

            return View(trips);
        }
        
        [Authorize]
        public HttpResponse Add()
        {
            if (!this.User.IsAuthenticated)
            {
                return Redirect("/Users/Login");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(AddTripFormModel model)
        {
            var modelErrors = this.validator.ValidateTrip(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

             DateTime.TryParseExact(model.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture,DateTimeStyles.None, out DateTime date);

            var trip = new Trip
            {
                StartPoint = model.StartPoint,
                EndPoint = model.StartPoint,
                DepartureTime = date,
                Seats = model.Seats,
                Description = model.Description
            };

            this.data.Trips.Add(trip);

            this.data.SaveChanges();

            return Redirect("/Trips/All");
        }

       
        [Authorize]
        [HttpPost]
        public HttpResponse Details(string Id)
        {
            var trip = this.data.
                Trips
                .Where(x=>x.Id==Id)
                .Select(x=> new TripDetailsFormModel
                {
                    Id=x.Id,
                    StartPoint=x.StartPoint,
                    EndPoint = x.EndPoint,
                    DepartureTime=x.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                    Description=x.Description,
                    Seats=x.Seats,
                    ImagePath=x.ImagePath

                }).FirstOrDefault();

            

            return this.View(trip);
        }
        [Authorize]
        [HttpPost]
        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!User.IsAuthenticated)
            {
                return Redirect("/");
            }

            var user = this.User;

            var userExistInTripCheck = this.data
                .UserTrips
                .FirstOrDefault(u => u.UserId == user.Id && u.TripId == tripId);

            if (userExistInTripCheck != null)
            {
                return this.Error("This trip has already been added");
            }

            var newUserToTrip = new UserTrip { TripId = tripId, UserId = user.Id };

            var currentTrip = this.data
                .Trips
                .FirstOrDefault(t => t.Id == tripId);

            currentTrip.Seats -= 1;

            if (currentTrip.Seats != 0 || currentTrip.Seats == 0)
            {
                this.data.Trips.Update(currentTrip);
                this.data.UserTrips.Add(newUserToTrip);

                this.data.SaveChanges();
            }
            else if (currentTrip.Seats < 0)
            {
                return this.Error("Тhere are no free seats");
            }

            return this.View(currentTrip, "Details");
        }

    }
}
