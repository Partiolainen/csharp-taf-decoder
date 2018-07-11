﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace csharp_taf_decoder.chunkdecoder
{
    public abstract class TafChunkDecoder : ITafChunkDecoder
    {
        /// <summary>
        /// Extract the corresponding chunk from the remaining taf.
        /// </summary>
        /// <param name="remainingTaf">matches array if any match (null if no match), + updated remaining taf</param>
        /// <returns></returns>
        public KeyValuePair<string, List<Group>> Consume(string remainingTaf)
        {
            var chunkRegex = new Regex(GetRegex());

            // try to match chunk's regexp on remaining taf
            var groups = chunkRegex.Match(remainingTaf).Groups.Cast<Group>().ToList();

            // consume what has been previously found with the same regexp
            var newRemainingTaf = chunkRegex.Replace(remainingTaf, string.Empty);

            return new KeyValuePair<string, List<Group>>(newRemainingTaf, groups);
        }

        protected Dictionary<string, object> GetResults(string newRemainingTaf, Dictionary<string, object> result)
        {
            //return result + remaining taf
            return new Dictionary<string, object>()
            {
                { TafDecoder.ResultKey, result },
                { TafDecoder.RemainingTafKey, newRemainingTaf }
            };
        }

        public abstract string GetRegex();

        public abstract Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false);
    }
}
