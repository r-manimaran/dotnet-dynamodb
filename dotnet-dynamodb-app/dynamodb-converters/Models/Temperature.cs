﻿using System.Text.Json.Serialization;

namespace dynamodb_converters.Models
{
    public class Temperature : ValueObject
    {
        public Temperature(Decimal degree,TemperatureType temperatureType) {
            Degree = degree;
            TemperatureType = temperatureType;
        }
        public decimal Degree { get; set; }
        public TemperatureType TemperatureType { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Degree;
            yield return TemperatureType;
        }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TemperatureType
    {
        Celcius,
        Farenheit
    }

    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, right) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
        // Other utility methods
    }
}
