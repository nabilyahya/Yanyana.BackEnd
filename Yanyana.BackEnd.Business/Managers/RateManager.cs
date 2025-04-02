using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Yanyana.BackEnd.Core.Entities;
using Yanyana.BackEnd.Data.Context;

namespace Yanyana.BackEnd.Business.Managers
{
    public interface IRateManager
    {
        Task CreateRateAsync(Rate rate);
        Task UpdateRateAsync(Rate updatedRate);
        Task DeleteRateAsync(int rateId);
        Task<Rate> GetRateByIdAsync(int rateId);
        Task<IEnumerable<Rate>> GetAllRatesAsync();
        Task<IEnumerable<Rate>> GetRatesByPlaceIdAsync(int placeId);
    }

    public class RateManager : IRateManager
    {
        private readonly YanDbContext _context;

        public RateManager(YanDbContext context)
        {
            _context = context;
        }

        public async Task CreateRateAsync(Rate rate)
        {
            if (rate == null)
                throw new ArgumentNullException(nameof(rate));

            ValidateRate(rate);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                rate.CreatedDate = DateTime.UtcNow;

                _context.Rates.Add(rate);
                int result = await _context.SaveChangesAsync();

                if (result <= 0)
                    throw new InvalidOperationException("Failed to create rate");

                transaction.Complete();
            }
        }

        public async Task UpdateRateAsync(Rate updatedRate)
        {
            if (updatedRate == null)
                throw new ArgumentNullException(nameof(updatedRate));

            ValidateRate(updatedRate);

            var existingRate = await _context.Rates
                .FirstOrDefaultAsync(r => r.RateId == updatedRate.RateId);

            if (existingRate == null)
                throw new KeyNotFoundException($"Rate with ID {updatedRate.RateId} not found");

            // Only update the properties that need to be changed
            existingRate.Value = updatedRate.Value;

            // Attach the entity to the context, and mark it as modified
            _context.Rates.Attach(existingRate);
            _context.Entry(existingRate).Property(r => r.Value).IsModified = true;

            int result = await _context.SaveChangesAsync();
            if (result <= 0)
                throw new InvalidOperationException("Failed to update rate");
        }

        public async Task DeleteRateAsync(int rateId)
        {
            var rate = await _context.Rates
                .Include(r => r.User)
                .Include(r => r.Place)
                .FirstOrDefaultAsync(r => r.RateId == rateId);

            if (rate == null)
                throw new KeyNotFoundException($"Rate with ID {rateId} not found");

            _context.Rates.Remove(rate);
            int result = await _context.SaveChangesAsync();

            if (result <= 0)
                throw new InvalidOperationException("Failed to delete rate");
        }

        public async Task<Rate> GetRateByIdAsync(int rateId)
        {
            return await _context.Rates
                .Include(r => r.User)
                .Include(r => r.Place)
                .FirstOrDefaultAsync(r => r.RateId == rateId);
        }

        public async Task<IEnumerable<Rate>> GetAllRatesAsync()
        {
            return await _context.Rates
                .Include(r => r.User)
                .Include(r => r.Place)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rate>> GetRatesByPlaceIdAsync(int placeId)
        {
            return await _context.Rates
                .Include(r => r.User)
                .Include(r => r.Place)
                .Where(r => r.PlaceId == placeId)
                .ToListAsync();
        }

        private void ValidateRate(Rate rate)
        {
            if (rate.Value < 1 || rate.Value > 5)
                throw new ArgumentException("Rate value must be between 1 and 5");

            if (rate.UserId <= 0)
                throw new ArgumentException("Invalid user ID");

            if (rate.PlaceId <= 0)
                throw new ArgumentException("Invalid place ID");
        }
    }
}
