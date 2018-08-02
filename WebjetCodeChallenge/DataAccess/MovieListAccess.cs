using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Newtonsoft.Json;
using RestSharp;
using WebjetCodeChallenge.Models;

namespace WebjetCodeChallenge.DataAccess
{
    public static class MovieListAccess
    {
        internal static Dictionary<string, MovieDetails> MovieDatabase = new Dictionary<string, MovieDetails>();

        public static void UpdateMovieDatabase()
        {
            var values = Enum.GetValues(typeof(Companies));
            foreach (var company in values)
            {
                var data = new List<ReturnedData>();

                try
                {
                    data = GetMovieList(company.ToString());
                }
                catch (NullReferenceException)
                {
                    Debug.WriteLine("Unable to Locate Movie Data");
                }

                foreach (ReturnedData movieDetails in data)
                {
                    try
                    {
                        if (MovieDatabase.ContainsKey(movieDetails.Title))
                        {
                            if (!MovieDatabase[movieDetails.Title].Company.Contains(company.ToString()))
                            {
                                AddToMaster(company.ToString(),
                                    GetMovieDetails(company.ToString(), movieDetails.ID));
                            }
                        }
                        else
                        {
                            AddToMaster(company.ToString(),
                                GetMovieDetails(company.ToString(), movieDetails.ID));
                        }
                    }
                    catch (NullReferenceException)
                    {
                        Debug.WriteLine("Unable to Locate Movie Data");
                    }
                }
            }
        }

        private static void AddToMaster(string comapny, ReturnedData data)
        {
            if (MovieDatabase.ContainsKey(data.Title))
            {
                if (!MovieDatabase[data.Title].Price.ContainsKey(comapny))
                {
                    MovieDatabase[data.Title].Price.Add(comapny, Convert.ToDouble(data.Price));
                }
                if (!MovieDatabase[data.Title].Id.ContainsKey(comapny))
                {
                    MovieDatabase[data.Title].Id.Add(comapny, data.ID);
                }
                MovieDatabase[data.Title].Company.Add(comapny);
            }

            else
            {
                var movie = new MovieDetails
                {
                    Title = data.Title,
                    Actors = data.Actors,
                    Awards = data.Awards,
                    Country = data.Country,
                    Director = data.Director,
                    Genre = data.Genre,
                    Language = data.Language,
                    Metascore = data.Metascore,
                    Plot = data.Plot,
                    Poster = data.Poster,
                    Rated = data.Rated,
                    Released = data.Released,
                    Runtime = data.Runtime,
                    Type = data.Type,
                    Writer = data.Writer,
                    Year = data.Year,
                    Rating = data.Rating,
                    Votes = data.Votes

                };
                movie.Id.Add(comapny, data.ID);
                movie.Price.Add(comapny, Convert.ToDouble(data.Price));
                movie.Company.Add(comapny);
                MovieDatabase.Add(data.Title, movie);
            }
        }

        public static List<ReturnedData> GetMovieList(string company)
        {
            //client and request setup
            var client =
                new RestClient(ConfigurationManager.AppSettings["RootApiAddress"] + company + "/movies/")
                {
                    Timeout = 3000
                };
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-access-token", ConfigurationManager.AppSettings["ApiKey"]);

            //perform request
            IRestResponse response = client.Execute(request);

            //convert response to object
            MovieModel data = JsonConvert.DeserializeObject<MovieModel>(response.Content);

            //gather extra information about movies

            return data.Movies;
        }

        public static ReturnedData GetMovieDetails(string company, string id)
        {
            var client =
                new RestClient(ConfigurationManager.AppSettings["RootApiAddress"] + company + "/movie/" +
                               id)
                { Timeout = 1000 };
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-access-token", ConfigurationManager.AppSettings["ApiKey"]);

            //perform request
            IRestResponse response = client.Execute(request);

            //convert response to object
            return response != null ? JsonConvert.DeserializeObject<ReturnedData>(response.Content) : new ReturnedData();
        }
    }
}