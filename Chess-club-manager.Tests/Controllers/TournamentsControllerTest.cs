using System;
using System.Web.Mvc;
using Chess_club_manager.Controllers;
using Chess_club_manager.DTO.Tournament.Tour;
using Chess_club_manager.Models;
using Chess_club_manager.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess_club_manager.Tests.Controllers
{
    [TestClass]
    public class TournamentsControllerTest
    {
        [TestMethod]
        public void TourDetails()
        {
            // Arrange - creating some mock repository and filling it with data
            var toursMock = new ToursMockRepository();

            var tId = 10;
            var tDate = DateTime.Now;

            var tMock = new TournamentTour()
            {
                Id = tId,
                CompletedDateTime = tDate,
                Tournament = new Tournament { Id = tId },
                TournamentId = tId
            };

            toursMock.Tours.Add(tMock);

            //creating a controller with mock data repository
            var controller = new TournamentsController(null, null, toursMock);
            
            // Act - simulating a
            var result = controller.TourDetails(id: tId) as ViewResult;
            

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.GetType(), typeof(ViewResult));

            var responseModel = result.Model as TourDetailsDto;
            Assert.IsNotNull(responseModel);

            Assert.AreEqual(responseModel.Id, tId);
            Assert.AreEqual(responseModel.CompletedDateTime, tDate);
            
        }

    }
}