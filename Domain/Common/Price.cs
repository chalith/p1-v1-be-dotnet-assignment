using System;
using System.Collections.Generic;
using Domain.SeedWork;

namespace Domain.Common
{
    public class Price : ValueObject
    {
        public decimal Value { get; private set; }
        public Currency Currency { get; private set; }

        protected Price() { }

        public Price(decimal value, Currency currency)
        {
            Value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }

        /**
        Add a value the current amount. New value = Old value + value.
        @param decimal value Value to add.
        **/
        public void AddValue(decimal value)
        {
            Value += value;
        }
    }
}
