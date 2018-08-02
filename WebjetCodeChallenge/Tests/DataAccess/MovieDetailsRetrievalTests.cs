using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebjetCodeChallenge.Models;

namespace WebjetCodeChallenge.DataAccess.Tests
{
    [TestClass()]
    public class MovieDetailsRetrievalTests
    {
        [TestMethod()]
        public void GetMovieDetailsTest()
        {
            //setup
            Array company = Enum.GetValues(typeof(Companies));
            const string id = "cw2488496";
            var expected = new ReturnedData()
            {
                Title = "Star Wars: The Force Awakens"
            };

            //perform
            ReturnedData actual = MovieListAccess.GetMovieDetails(company.GetValue(0).ToString(), id);

            //assert 
            Assert.AreEqual(expected.Title, actual.Title);
        }

        [TestMethod()]
        public void GetMovieListTest()
        {
            //setup
            Array company = Enum.GetValues(typeof(Companies));

            //perform
            var actual = MovieListAccess.GetMovieList(company.GetValue(0).ToString());

            //assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);

        }
    }
}