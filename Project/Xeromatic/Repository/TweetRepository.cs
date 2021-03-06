﻿using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Xeromatic.Models;

namespace Xeromatic.Repository
{
    public class TweetRepository : ITweetRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["tweetDB"].ConnectionString;

        public IEnumerable<TweetOutput> GetAllTweets()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<TweetOutput>(@"SELECT Id, TweetText, RetweetCount, TweetImage, CreationTime 
                                                    FROM dbo.Tweets");
            }
        }

        public void Insert(TweetInput tweet)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO Tweets (Id, TweetText, RetweetCount, TweetImage, CreationTime) " +
                                   "VALUES(@Id, @TweetText, @RetweetCount, @TweetImage, @CreationTime); ", 
                    new { Id=tweet.Id, TweetText=tweet.TweetText,
                        RetweetCount=tweet.RetweetCount,
                        TweetImage=tweet.TweetImage,
                        CreationTime=tweet.CreationTime });
            }
        }

        public void Delete(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Tweets WHERE Id=@id; ", new { id });
            }
        }
    }
}