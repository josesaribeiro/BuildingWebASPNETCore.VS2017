﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting All Trips from database");
            return _context
                   .Trips
                   .Include(t => t.Stops)
                   .ToList();
        }

        public IEnumerable<Trip> GetTripsByUsername(string username)
        {
            _logger.LogInformation("Getting All Trips ByUsername from database");
            return _context
                .Trips
                .Where(t => t.UserName == username)
                .ToList();
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName)
                .FirstOrDefault();
        }

        public Trip GetUserTripByName(string tripName, string username)
        {
            return _context.Trips
                    .Include(t => t.Stops)
                    .Where(t => t.Name == tripName && t.UserName == username)
                    .FirstOrDefault();
        }


        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }
        public void AddStop(string tripName, Stop newStop, string username)
        {
            var trip = GetUserTripByName(tripName, username);

            if (trip != null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }


    }
}
