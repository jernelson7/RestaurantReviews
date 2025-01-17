﻿using RestaurantReviews.Core.DataTypes;
using RestaurantReviews.Core.Interfaces;
using RestaurantReviews.Database.Sqlite;
using SQLite.Net.Platform.Win32;
using System;
using System.Collections.Generic;

namespace RestaurantReviews.Controller
{
    public class RestaurantReviewsController : IRestaurantReviewController
    {
        private IRestaurantReviewDatabase _db;

        public RestaurantReviewsController(string dbFilePath)
        {
            _db = new SqliteRestaurantReviewDatabase(new SQLitePlatformWin32(), dbFilePath);
        }

        public void AddRestaurant(IRestaurant restaurant)
        {
            _db.AddRestaurant(restaurant);
        }

        public void DeleteRestaurant(Guid restaurantId)
        {
            _db.DeleteRestaurant(restaurantId);
        }

        public void AddReview(IRestaurantReview review)
        {
            if (string.IsNullOrEmpty(review.ReviewText))
                throw new InvalidEntryException($"{nameof(review.ReviewText)} cannot be empty");

            if (review.FiveStarRating < 1 || review.FiveStarRating > 5)
                throw new InvalidEntryException($"{nameof(review.FiveStarRating)} must be between 1 and 5 (inclusive)");

            _db.AddReview(review);
        }

        public void DeleteReview(Guid reviewId)
        {
            _db.DeleteReview(reviewId);
        }

        public IRestaurant GetRestaurant(Guid restaurantId)
        {
            return _db.GetRestaurant(restaurantId);
        }

        public IRestaurantReview GetReview(Guid reviewId)
        {
            return _db.GetReview(reviewId);
        }

        public IReadOnlyList<IRestaurant> FindRestaurants(RestaurantsQuery query)
        {
            return _db.FindRestaurants(query.Name, query.City, query.StateOrProvince, query.PostalCode);
        }

        public IReadOnlyList<IRestaurantReview> GetReviewsForRestaurant(Guid restaurantId)
        {
            return _db.GetReviewsForRestaurant(restaurantId);
        }

        public IReadOnlyList<IRestaurantReview> GetReviewsForUser(Guid reviewerId)
        {
            return _db.GetReviewsForReviewer(reviewerId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db?.Dispose();
                _db = null;
            }
        }
    }
}
