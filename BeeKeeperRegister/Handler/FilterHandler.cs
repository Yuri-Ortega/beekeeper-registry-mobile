using BeeKeeperRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Handler
{
    public class FilterHandler
    {
        /// <summary>
        /// Filters a collection of bee types, excluding duplicates based on location and production type, except for
        /// the current bee type.   
        /// </summary>
        /// <param name="all">The complete collection of bee types to filter.</param>
        /// <param name="temp">A collection representing existing bee type and location/production type associations.</param>
        /// <param name="currentBeeTypeId">The identifier of the current bee type to always include.</param>
        /// <param name="provCode">The province code used to check for duplicates.</param>
        /// <param name="countryCode">The country code used to check for duplicates.</param>
        /// <returns>A filtered collection of bee types with duplicates removed except for the current bee type.</returns>
        public static IEnumerable<BeeTypesModel> FilterBeeTypes(
        IEnumerable<BeeTypesModel> all,
        IEnumerable<BeeLocationProductionTypeSourceModel> temp,
        string currentBeeTypeId,
        string? provCode,
        string? countryCode)
        {
            return all.Where(item =>
            {
                bool isDuplicate = temp.Any(x =>
                    x.BeeTypeId == item.BeeTypeId &&
                    (
                        (!string.IsNullOrEmpty(provCode) && x.ProvCode == provCode) ||
                        (!string.IsNullOrEmpty(countryCode) && x.Gmicntry == countryCode)
                    )
                );

                bool isCurrent = item.BeeTypeId == currentBeeTypeId;


                return !isDuplicate || isCurrent;
            });
        }

        /// <summary>
        /// Filters bee types by excluding those that are duplicates in the provided temporary species list for the
        /// specified province or country, except for the current bee type.
        /// </summary>
        /// <param name="all">The complete list of bee types to filter.</param>
        /// <param name="temp">The temporary list of bee species used to identify duplicates.</param>
        /// <param name="currentBeeTypeId">The ID of the current bee type to always include.</param>
        /// <param name="provCode">The province code used to match duplicates.</param>
        /// <param name="countryCode">The country code used to match duplicates.</param>
        /// <returns>A filtered collection of bee types excluding duplicates except for the current bee type.</returns>
        public static IEnumerable<BeeTypesModel> TempFilterBeeTypes(
        IEnumerable<BeeTypesModel> all,
        IEnumerable<TempDataBeeSpeciesModel> temp,
        string currentBeeTypeId,
        string? provCode,
        string? countryCode)
        {
            return all.Where(item =>
            {
                bool isDuplicate = temp.Any(x =>
                    x.BeeTypeId == item.BeeTypeId &&
                    (
                        (!string.IsNullOrEmpty(provCode) && x.ProvCode == provCode) ||
                        (!string.IsNullOrEmpty(countryCode) && x.Gmicntry == countryCode)
                    )
                );

                bool isCurrent = item.BeeTypeId == currentBeeTypeId;


                return !isDuplicate || isCurrent;
            });
        }

        /// <summary>
        /// Parses a comma-separated string into a set of distinct, trimmed, lowercase tokens.
        /// </summary>
        /// <param name="data">A comma-separated string containing tokens to filter.</param>
        /// <returns>A HashSet containing the filtered tokens, or an empty set if the input is null or empty.</returns>
        public static HashSet<string> FilterMultipleSelectionTokenEdit(string data)
        {
            return data?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLower())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToHashSet()
                ?? new HashSet<string>();
        }

        /// <summary>
        /// Removes any text in the input string following an opening parenthesis and trims whitespace.
        /// </summary>
        /// <param name="species">The species name to process.</param>
        /// <returns>A trimmed species name without any summary in parentheses.</returns>
        public static string RemoveSpacingBeeSummary(string species)
        {
            if (string.IsNullOrEmpty(species))
                return string.Empty;

            int index = species.IndexOf("(");
            if (index > 0)
                return species.Substring(0, index).Trim();

            return species.Trim();
        }

        /// <summary>
        /// Removes any text in the input string following an opening parenthesis and trims whitespace.
        /// </summary>
        /// <param name="species">The species name to process.</param>
        /// <returns>A trimmed species name without any text in parentheses.</returns>
        public static string RemoveParenthesis(string species)
        {
            if (string.IsNullOrEmpty(species)) return string.Empty;
            int index = species.IndexOf("(");
            return index > 0
                ? species[..index].Trim()
                : species.Trim();
        }
    }
}
