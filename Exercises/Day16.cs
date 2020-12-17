﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventCode.Exercises
{
    public static class Day16
    {
        private static IList<Rule> GetParsedRules()
        {
            var splitRules = RulesString.Split("\r\n");

            var rules = splitRules.Select(x => 
            {
                var splitRule = x.Split(":");

                var name = splitRule.First();
                var rangesSplit = splitRule.ElementAt(1)
                                .Split("or")
                                .Select(y => 
                                {
                                    var ranges = y.Split("-");
                                    var start = int.Parse(ranges.First());
                                    var end = int.Parse(ranges.Last());

                                    return (start, end);
                                }).ToList();

                return new Rule 
                { 
                    Name = name, 
                    MinRange = rangesSplit.First(),
                    MaxRange = rangesSplit.Last(),
                     };
            }).ToList();

            return rules;
        }

        private static string[] GetNearbyTickets => NearbyTickets.Split("\r\n");        

        private static IList<int> GetNearbyTicketsValues()
        {
            var split = GetNearbyTickets;

            var aux = split.SelectMany(
                x => x.Split(","),
                (_, value) => 
                {
                    return int.Parse(value);
                }).ToList();

            return aux;
        }

        private static IEnumerable<int> GetEnumerableRange((int, int) range)
        {
            var count = range.Item2 - range.Item1 + 1;
            var result = Enumerable.Range(range.Item1, count);

            return result;
        }
        
        private static IEnumerable<int> GetPosibleNumbers(Rule rule)
        {
            var minRange = GetEnumerableRange(rule.MinRange);
            var maxRange = GetEnumerableRange(rule.MaxRange);

            var result = minRange.Concat(maxRange);
            return result;
        }

        private static IList<int> GetNotPossibleValues()
        {
            var rules = GetParsedRules();
            var nearbyTicketsValues = GetNearbyTicketsValues();

            var posibleValues = new HashSet<int>();
            foreach (var rule in rules)
            {
                var enumerableRange = GetPosibleNumbers(rule);
                posibleValues.UnionWith(enumerableRange);
            }
            var possibleValuesList = posibleValues.ToList();
            possibleValuesList.Sort();
            var notPosibleValues = nearbyTicketsValues
                                    .Where(x => !posibleValues.Contains(x))
                                    .ToList();

            return notPosibleValues;
        }

        public static long Exercise1()
        {
            long result = 0;

            result = GetNotPossibleValues().Sum();

            return result;
        }

        public static long Exercise2()
        {
            long result = 0;

            // Parsed rules.
            var rules = GetParsedRules();

            // Get not possible values, calculated in first exercise.
            var notPossibleValues = GetNotPossibleValues().ToList();

            // Get list of parsed values in nearby ticket data.
            var nearbyTicketsList = GetNearbyTickets.Select(x => 
            {
                var numbers = x.Split(",").Select(y => int.Parse(y)).ToArray();

                return numbers;
            });

            // Get all the nearby tickets list wihtout the ones that contain not possible values.
            var validTicketList = nearbyTicketsList.Where(x => !x.Any(y => notPossibleValues.Contains(y)));

            // Parse our ticket and append.
            var ticket = Ticket.Split(",").Select(x => int.Parse(x));
            //validTicketList.Append(ticket);

            // Get number of fields.
            var numberOfRows = rules.Count();

            List<int[]> ticketColumns = new List<int[]>();

            // Transpose matrix.
            for (int i = 0; i < numberOfRows; i++)
            {
                var column = validTicketList.Select(x => x.ElementAt(i));
                ticketColumns.Add(column.ToArray());
            }

            IEnumerable<Rule> pendingIndexRules = rules;
            var pendingTicketColums = new List<int[]>(ticketColumns);
            do
            {                
                // Iterate rules.
                foreach (var rule in pendingIndexRules)
                {
                    // Possible numbers for rule.
                    var possibleValues = GetPosibleNumbers(rule);

                    // Get all columns that fits with possible values.
                    var possibleRows = pendingTicketColums.Where(x => x.All(y => possibleValues.Contains(y)));
                    var possibleRowsCount = possibleRows.Count();

                    if (possibleRowsCount == 1)
                    {
                        // Assign index, mark as done and remove from pending.
                        rule.index = ticketColumns.IndexOf(possibleRows.First());
                        rule.hasIndex = true;
                        pendingTicketColums.Remove(possibleRows.First());
                        break;
                    }
                }
                // Remove already assigned rule.
                pendingIndexRules = rules.Where(x => !x.hasIndex).ToList();

            } while (pendingIndexRules.Any()); // Repeat while still pending rules to find index.

            // Get all rule indexes that have 'departure' in its name.
            var departureIndexes = rules.Where(x => x.Name.Contains("departure", StringComparison.OrdinalIgnoreCase))
                                    .Select(x => x.index);

            // Get values of own ticket , cast to long and multiply each other.
            result = departureIndexes.Select(x => (long)ticket.ElementAt(x))
                                     .Aggregate((a,b) => a*b );

            return result;
        }

        private class Rule
        {
            public string Name { get; set; }
            public (int, int) MinRange { get; set; }
            public (int, int) MaxRange { get; set; }
            public bool hasIndex { get; set; } = false;
            public int index { get; set; }
        }

        private static readonly string RulesString =
            @"departure location: 27-840 or 860-957
departure station: 28-176 or 183-949
departure platform: 44-270 or 277-967
departure track: 33-197 or 203-957
departure date: 47-660 or 677-955
departure time: 45-744 or 758-971
arrival location: 42-636 or 642-962
arrival station: 44-243 or 252-962
arrival platform: 46-428 or 449-949
arrival track: 25-862 or 876-951
class: 26-579 or 585-963
duration: 38-683 or 701-949
price: 41-453 or 460-970
route: 48-279 or 292-963
row: 33-617 or 637-955
seat: 39-328 or 351-970
train: 35-251 or 264-957
type: 25-380 or 389-951
wagon: 42-461 or 480-965
zone: 33-768 or 789-954";

        private static readonly string Ticket =
            @"83,53,73,139,127,131,97,113,61,101,107,67,79,137,89,109,103,59,149,71";

        private static readonly string NearbyTickets =
            @"541,797,657,536,243,821,805,607,97,491,714,170,714,533,363,491,896,399,710,865
351,879,143,113,228,415,393,714,163,171,233,726,422,469,706,264,83,354,309,915
590,56,148,311,729,76,884,352,590,419,205,393,287,761,305,838,76,762,390,914
77,828,197,946,568,599,610,145,307,741,536,4,617,491,158,879,895,794,718,219
828,483,885,827,313,539,602,928,604,74,199,595,733,519,238,909,681,77,238,356
881,557,93,240,193,509,488,767,561,912,553,909,333,185,510,428,720,804,409,765
944,242,652,683,876,592,315,225,378,231,862,975,647,371,707,657,726,937,577,509
251,196,933,499,911,718,800,71,128,891,595,723,121,401,158,494,928,886,212,937
406,168,815,115,643,95,909,878,313,50,656,146,706,904,300,177,505,114,799,521
400,76,450,113,305,98,490,558,475,941,449,104,139,392,508,790,571,79,85,557
488,378,898,461,737,127,144,99,221,212,555,174,454,535,717,736,209,216,928,643
405,532,390,61,118,838,633,506,304,726,266,502,306,311,144,88,74,586,171,654
423,890,925,611,63,762,169,659,318,539,498,404,926,814,541,619,404,392,655,118
51,899,946,736,429,701,547,603,893,946,609,300,568,361,50,299,902,905,311,94
394,556,125,351,366,940,589,215,5,243,570,725,452,794,767,610,524,645,823,613
899,268,420,238,161,602,113,363,322,74,363,413,140,208,11,374,491,322,376,184
415,922,592,98,729,352,511,879,846,523,145,219,301,357,820,163,268,145,320,307
201,484,234,368,451,322,316,763,299,902,815,159,116,133,419,912,389,314,585,120
884,353,903,83,396,417,652,807,223,659,538,540,943,377,151,130,908,347,120,302
598,712,636,711,610,94,558,409,95,659,713,918,509,120,169,99,370,352,563,135
319,874,277,419,324,264,534,600,481,313,176,815,67,660,493,389,616,91,827,719
654,167,659,104,932,571,300,737,226,758,996,369,565,460,220,140,568,709,461,322
112,943,186,293,499,267,209,517,940,449,493,407,261,216,397,881,813,707,95,824
530,409,499,390,477,801,398,375,940,518,426,215,421,910,79,206,461,115,942,532
126,573,654,75,124,218,647,766,542,734,577,224,679,911,107,546,418,199,394,236
136,892,204,806,125,389,876,834,159,185,549,280,140,175,792,553,69,299,397,154
724,654,937,593,677,998,215,605,766,823,174,358,561,277,401,309,239,758,196,807
653,534,574,999,208,767,206,405,565,718,73,908,558,914,360,50,80,208,889,268
600,99,145,391,371,141,710,643,882,592,232,579,370,710,390,672,185,485,702,399
887,78,156,558,415,681,63,242,99,57,603,451,327,821,229,612,602,758,721,747
644,561,312,306,531,809,88,212,550,794,169,813,553,373,845,376,701,808,318,306
289,708,731,551,65,358,63,791,725,765,190,609,143,706,306,732,410,603,517,929
723,605,561,103,4,679,524,124,159,57,239,561,481,507,313,724,502,826,154,821
210,301,0,94,89,730,932,939,401,105,569,378,197,703,313,327,642,410,512,503
608,345,538,241,90,896,50,827,536,803,921,902,535,723,359,482,935,122,704,266
734,589,417,279,167,730,312,86,159,813,733,305,511,611,425,756,268,602,915,587
409,503,521,205,714,416,642,488,191,896,922,504,887,134,589,948,889,90,802,472
206,70,82,710,318,418,411,821,374,925,150,865,320,568,376,725,909,559,128,67
368,576,899,325,833,871,84,520,762,183,404,216,545,888,317,679,908,742,52,409
733,268,265,579,419,557,948,125,563,239,659,727,571,997,537,609,364,322,160,53
662,948,394,209,905,811,55,495,509,828,405,233,150,307,826,608,93,714,449,485
126,643,811,679,714,373,549,270,198,225,103,327,824,270,902,936,703,828,326,647
838,89,562,419,325,410,800,196,805,586,976,703,107,589,861,796,354,861,322,514
237,682,935,403,410,942,554,324,75,352,702,313,486,680,183,251,570,892,813,572
252,103,64,712,713,604,197,805,67,305,914,727,556,702,224,361,151,65,601,97
943,498,499,537,140,305,305,679,550,353,76,57,571,825,436,413,107,394,415,190
721,906,69,487,88,389,928,552,525,537,320,730,529,309,584,731,321,708,715,56
140,491,884,567,929,228,571,109,915,509,817,832,570,913,19,120,710,609,827,883
797,60,491,351,551,498,90,646,278,569,715,406,492,374,177,194,554,54,794,87
577,550,223,110,917,563,815,326,149,241,17,878,222,683,913,399,606,207,541,613
900,752,730,709,380,593,324,236,533,726,306,400,173,650,66,570,102,278,830,307
145,650,614,559,948,496,57,562,533,299,527,795,568,505,749,354,91,215,81,115
543,373,520,703,120,711,768,91,112,152,308,876,479,126,486,69,303,209,836,825
328,325,596,84,683,267,393,892,701,184,373,565,654,505,647,597,23,828,188,536
876,653,389,4,66,615,220,485,416,513,923,60,796,804,942,172,917,587,789,838
367,231,705,936,933,524,361,920,439,293,302,941,50,792,678,721,211,650,609,188
839,676,501,536,482,929,508,213,926,542,115,105,419,298,921,537,761,737,223,147
820,561,768,529,293,304,121,51,338,220,214,206,701,524,243,578,938,918,306,406
91,204,805,555,911,379,304,883,458,914,562,813,205,143,531,650,325,410,71,80
547,93,227,402,735,226,194,426,721,741,515,96,426,554,616,289,324,916,218,923
940,537,535,552,905,65,568,949,634,94,523,679,898,103,938,157,191,265,709,713
311,216,653,789,317,506,515,492,551,976,826,389,532,609,312,912,762,132,743,944
452,605,325,834,921,592,888,760,917,176,534,704,805,895,280,266,97,131,130,98
80,564,417,795,819,543,554,313,816,150,677,589,653,600,609,276,735,101,906,907
16,574,153,592,135,220,372,562,152,876,919,395,768,600,235,542,412,297,522,227
173,86,76,875,157,732,452,173,648,789,649,326,649,613,68,413,84,410,744,53
514,17,314,413,292,517,450,890,575,710,391,269,564,880,916,367,809,111,898,814
240,603,515,367,719,198,883,558,936,391,449,295,743,241,412,496,372,896,594,679
188,575,817,818,558,494,69,586,169,596,134,394,353,335,421,800,565,293,304,563
416,474,763,323,759,588,883,526,902,827,528,102,500,353,542,234,681,615,822,495
597,107,369,948,546,292,571,530,944,92,916,118,792,767,204,61,273,316,919,490
891,642,127,830,544,483,104,494,928,223,395,906,665,114,941,323,132,75,573,560
212,75,485,53,740,185,488,539,861,303,102,199,183,799,490,945,352,428,238,157
321,127,882,657,212,236,425,188,908,941,657,662,933,295,554,352,104,101,531,358
698,105,705,604,129,325,452,597,575,574,129,680,400,366,427,103,125,121,266,566
106,151,175,61,212,806,276,453,370,806,494,832,175,321,265,807,539,495,139,159
379,568,186,587,93,947,748,495,53,940,727,882,542,143,918,896,484,395,137,213
575,536,486,83,217,58,169,98,577,679,517,878,854,705,736,731,96,51,301,807
722,921,188,807,488,555,311,170,643,480,229,736,16,266,426,501,900,100,593,65
206,814,449,789,116,236,3,679,133,87,495,391,312,596,567,427,417,121,183,795
107,326,96,399,149,485,204,215,171,570,644,517,275,837,921,533,231,295,128,293
106,151,74,607,920,391,50,320,680,411,720,629,826,547,514,277,227,277,566,552
301,148,917,682,155,121,712,909,495,105,842,195,239,313,764,234,906,302,209,119
795,278,913,612,807,223,301,547,608,351,185,297,808,941,775,269,93,449,705,135
406,509,678,608,365,153,81,608,426,409,940,533,626,124,818,577,105,650,134,266
10,116,99,838,568,356,375,944,564,823,947,824,542,553,939,534,371,507,905,882
544,74,827,77,164,224,169,84,751,593,730,512,300,556,227,152,167,538,225,531
173,368,680,210,343,680,323,605,159,549,558,509,560,414,82,601,762,702,920,657
611,515,735,306,71,363,921,456,926,142,586,614,129,566,293,948,304,564,71,372
106,130,372,363,561,305,942,75,683,549,216,394,363,719,831,420,626,763,96,63
738,546,896,602,198,943,601,54,526,316,924,548,677,410,489,85,373,85,371,798
941,136,474,881,510,314,461,301,491,500,105,540,309,375,234,575,879,576,795,389
132,516,568,324,390,835,526,243,205,155,77,656,688,678,487,185,231,792,890,916
727,807,221,14,836,763,450,727,368,889,547,229,592,127,106,424,86,279,143,193
178,833,228,212,794,534,321,710,544,410,644,531,317,712,722,714,484,616,265,404
570,410,948,312,823,678,811,843,833,794,802,142,366,82,235,712,142,884,367,510
378,902,278,796,358,396,421,936,564,836,935,852,707,732,428,371,304,228,681,924
794,125,133,598,867,88,278,192,525,564,647,551,109,876,119,577,740,196,596,562
742,649,923,596,408,497,151,294,135,542,892,402,122,146,393,491,871,651,655,862
720,677,372,191,573,11,114,413,74,395,721,416,365,193,812,131,91,643,360,608
235,658,490,118,498,804,243,181,505,861,81,396,505,595,817,742,411,733,678,215
236,420,146,210,524,598,741,94,144,140,389,415,267,400,647,689,811,743,211,553
545,418,799,653,650,567,560,68,551,102,376,339,511,267,503,130,890,374,51,118
520,701,547,509,528,588,196,16,278,713,643,797,163,718,650,886,830,561,512,714
811,129,311,75,506,370,377,825,540,59,720,710,609,765,258,170,224,173,819,299
560,532,997,711,546,59,499,163,412,482,235,83,681,52,922,646,316,899,160,659
552,885,703,793,650,91,390,210,612,376,422,471,677,495,300,919,141,712,392,496
907,575,898,924,594,742,875,418,98,56,160,643,826,139,123,533,208,73,794,412
212,76,421,74,548,896,830,238,209,730,394,648,199,265,352,552,509,138,213,616
604,391,822,380,60,176,277,814,893,359,652,893,818,934,201,944,131,547,732,64
947,824,609,70,362,608,569,588,911,828,218,612,401,451,244,586,130,209,501,270
807,761,525,157,417,749,529,655,422,302,190,395,892,145,947,417,129,917,511,828
279,378,717,948,630,647,145,351,937,617,209,893,744,366,176,491,568,233,420,173
535,889,730,766,96,11,133,903,113,679,762,940,512,99,176,593,899,78,534,810
461,533,427,491,711,470,214,363,205,878,508,401,77,103,884,816,566,147,183,890
391,824,701,494,594,596,61,418,103,22,193,924,577,419,918,450,927,884,524,494
110,594,156,727,274,146,655,814,819,352,518,934,52,939,323,567,740,839,899,810
731,60,568,878,241,213,146,544,732,184,133,556,732,203,634,562,228,884,681,600
761,725,169,926,239,223,428,828,880,900,491,557,947,268,882,429,324,656,399,485
409,518,365,235,449,730,895,899,867,296,518,361,138,506,365,520,682,514,928,713
717,104,406,834,95,132,126,765,613,356,866,818,323,714,152,411,650,377,923,193
96,209,87,113,392,98,392,895,907,840,296,228,650,208,303,872,409,893,270,825
652,367,417,899,507,375,526,231,806,67,545,826,76,306,687,400,885,554,295,652
722,514,361,916,517,220,236,657,801,878,458,102,367,117,543,212,304,364,571,60
827,53,511,882,589,214,302,949,512,807,664,150,243,394,131,159,106,317,372,554
496,496,866,298,104,803,502,189,561,402,909,739,241,901,545,498,922,123,522,577
578,178,230,528,325,765,723,946,600,176,242,123,599,231,113,925,461,357,531,612
539,621,683,505,823,597,86,742,513,710,763,497,161,830,374,709,543,138,816,323
365,274,487,734,302,593,590,57,511,423,643,131,379,887,701,154,292,380,730,759
163,896,825,194,407,806,596,122,557,417,179,766,565,644,234,491,653,916,899,131
738,482,733,678,391,744,650,830,544,578,604,830,328,490,574,380,726,204,765,630
554,217,564,809,702,178,798,216,73,111,67,165,153,612,267,879,319,895,328,503
323,541,221,420,918,239,450,512,231,118,279,542,297,814,663,766,317,322,801,226
929,590,136,222,642,596,879,111,275,728,425,175,523,423,607,789,647,143,522,892
300,184,498,154,457,899,224,574,878,204,789,297,101,493,653,303,169,389,833,416
54,496,607,898,3,133,791,558,917,828,190,81,861,310,810,225,521,105,725,831
73,460,927,303,215,79,926,894,220,575,910,548,893,395,520,812,472,716,526,876
233,907,216,95,821,537,155,831,327,703,703,219,171,993,935,306,818,560,718,502
293,545,890,893,653,272,492,825,881,305,126,119,482,739,138,609,818,516,358,886
720,929,314,537,313,231,722,199,876,572,414,642,586,884,817,94,134,172,759,421
62,789,808,939,554,588,713,375,226,821,536,603,736,203,821,610,178,742,295,409
134,828,762,830,524,355,555,948,824,230,806,150,640,911,530,189,608,61,925,513
657,681,517,576,126,683,310,924,766,822,544,445,361,493,821,372,928,903,530,556
316,107,507,920,111,21,912,232,543,893,353,803,521,396,53,538,528,117,766,826
84,947,862,567,281,836,801,84,197,562,317,481,938,394,215,649,550,831,617,758
124,977,495,880,192,362,605,192,758,759,59,556,483,135,216,238,907,317,910,501
320,683,353,358,277,815,934,266,159,614,239,257,185,149,404,239,885,169,801,533
85,511,481,460,766,396,499,403,420,768,919,646,359,826,708,903,902,476,141,117
453,396,173,103,729,228,911,667,296,574,277,821,546,808,210,106,556,552,278,324
572,508,894,606,826,910,325,427,740,63,491,860,429,494,646,806,298,482,527,885
802,227,143,726,390,592,931,61,179,653,380,896,219,920,400,929,839,823,497,701
493,204,817,489,234,69,229,319,800,808,421,813,775,607,735,792,419,494,219,210
79,923,359,391,240,914,85,529,450,577,274,397,140,731,194,862,240,730,553,894
803,316,606,53,130,940,861,728,401,796,86,566,132,194,66,567,350,704,832,765
642,803,395,408,732,151,79,341,596,269,587,106,297,150,83,943,175,327,537,943
549,915,678,224,656,82,159,239,574,534,724,459,607,550,614,565,360,376,68,933
136,520,654,707,930,121,471,943,415,720,646,135,195,214,489,268,644,63,939,187
538,317,162,917,990,377,528,366,937,174,165,894,563,572,265,835,173,922,529,373
134,321,614,617,420,914,818,113,572,218,746,91,321,741,729,913,806,489,237,189
735,76,415,989,243,881,321,505,427,555,922,713,551,172,558,59,736,402,98,560
543,76,645,106,212,742,74,304,192,237,115,274,379,359,529,650,549,678,803,924
942,566,933,836,831,292,108,678,324,574,67,178,63,229,67,269,733,513,356,424
515,88,538,172,12,829,563,733,564,896,205,118,174,228,58,606,720,355,188,714
480,52,570,115,214,550,518,168,79,131,315,726,654,211,890,789,149,978,235,536
442,427,93,427,209,451,378,830,860,808,364,820,68,766,268,410,141,125,52,930
118,62,61,418,177,303,157,396,494,947,806,304,144,407,192,138,389,557,723,159
815,904,922,560,148,542,308,492,558,50,619,711,378,235,396,212,768,838,497,419
792,884,758,925,390,103,997,511,743,234,860,146,186,421,203,62,161,186,326,601
919,206,266,570,707,678,767,884,818,710,560,133,879,145,720,526,915,620,299,511
918,579,380,404,547,143,812,942,81,592,408,602,891,642,728,86,886,63,492,343
237,389,64,462,838,881,656,893,839,318,920,906,720,215,184,57,88,562,527,267
799,353,728,363,96,299,822,205,282,566,93,936,362,607,921,517,578,703,268,191
819,152,617,937,642,738,197,560,174,426,603,508,804,739,722,458,806,264,491,726
905,861,108,793,295,323,840,74,663,654,327,565,129,161,836,896,92,427,652,837
917,232,88,203,900,174,604,139,575,943,924,606,8,605,482,267,537,209,377,724
520,838,175,895,881,895,929,629,373,499,717,832,704,642,452,379,617,944,559,649
883,193,902,293,897,265,946,238,295,117,442,701,140,120,132,815,655,411,725,526
646,226,907,65,807,191,399,275,677,175,223,790,656,393,371,809,102,740,647,313
125,302,943,278,593,297,350,72,416,861,377,791,266,277,726,325,188,554,794,308
217,357,79,371,263,921,237,714,830,521,124,184,154,73,677,59,507,563,556,308
918,243,797,94,98,665,168,92,594,213,608,798,377,652,715,502,939,926,170,168
683,373,302,814,423,898,264,722,723,601,575,164,327,303,927,201,568,240,241,313
268,86,163,511,713,933,372,877,98,243,397,927,660,818,157,165,588,510,980,84
328,116,882,334,561,209,791,312,100,76,548,614,941,894,365,174,305,508,125,706
92,165,73,654,362,217,97,450,981,327,718,911,147,831,741,53,723,239,60,415
365,312,556,765,766,378,946,587,372,126,901,553,873,903,945,143,739,159,147,806
812,189,293,421,231,452,876,522,392,679,314,392,217,512,637,102,828,566,520,679
51,861,740,706,551,196,327,301,400,797,585,532,209,107,504,911,752,823,328,510
948,551,419,929,146,533,197,559,610,407,343,230,542,304,741,718,537,578,701,544
454,797,235,743,189,120,789,154,883,915,547,61,305,913,540,564,882,300,743,878
567,321,767,497,82,856,483,58,238,837,932,900,930,830,390,799,225,323,312,485
272,803,572,210,901,652,362,426,309,900,790,522,98,569,812,216,356,146,79,941
820,796,532,929,368,83,213,885,738,61,191,698,70,155,230,500,208,721,108,520
319,242,921,514,324,710,323,560,325,84,551,914,833,156,795,783,899,602,368,535
800,482,567,307,101,545,233,815,937,792,376,723,811,400,311,462,428,196,607,940
722,85,369,612,213,125,157,835,452,412,763,210,266,306,273,725,544,227,655,924
498,718,826,488,731,191,392,510,835,758,791,165,919,427,523,856,172,797,729,764
680,811,939,190,761,711,68,563,932,75,551,553,303,119,112,724,763,172,364,198
756,73,95,760,127,503,744,172,921,406,704,294,81,924,653,839,592,738,135,911
942,191,159,926,214,176,396,423,579,879,511,103,749,795,302,323,832,920,425,378
335,551,205,903,502,212,392,88,561,579,915,815,357,169,189,728,657,611,790,359
578,111,934,497,109,344,840,360,761,320,935,588,393,158,128,887,310,837,136,541
877,789,125,482,305,828,202,493,148,94,889,824,481,116,305,305,124,220,608,107
58,75,136,545,551,613,313,566,723,107,112,754,534,605,122,67,220,194,716,268
617,241,488,102,605,837,209,93,917,173,305,813,834,522,360,370,391,874,452,81
470,113,657,164,313,657,760,511,653,914,116,519,908,703,213,729,197,924,267,354
528,201,376,143,461,172,237,648,519,364,707,224,68,706,817,767,91,219,113,452
729,937,554,186,721,720,886,467,221,814,761,375,681,736,607,144,398,901,104,225
231,153,537,225,886,368,567,882,397,331,914,588,355,796,737,800,154,920,533,157
364,391,677,758,372,551,653,545,311,926,307,893,358,238,94,124,880,498,483,23
914,653,711,311,172,593,833,921,948,861,299,907,920,911,561,256,607,607,221,557
733,188,102,658,527,209,54,948,611,294,216,400,742,57,938,243,201,732,375,649
117,416,151,554,900,539,547,719,451,316,112,307,234,171,927,336,820,484,725,497
54,114,403,913,120,53,243,112,600,721,94,146,738,22,213,311,173,894,546,526
632,407,615,929,409,551,729,911,801,229,918,512,240,767,378,267,929,121,148,95
214,522,421,816,576,904,789,152,114,358,506,7,361,915,949,424,892,911,726,205
800,554,975,682,532,716,295,585,919,453,904,600,508,391,149,577,371,817,122,163
134,108,71,578,742,573,206,793,405,890,710,660,94,538,470,762,394,606,369,368
150,793,728,138,768,632,839,565,937,164,714,317,615,369,323,228,268,793,86,920
503,596,527,66,71,168,525,801,391,491,572,829,196,158,865,318,397,607,823,562
90,143,53,300,536,983,597,88,825,105,946,568,320,408,327,400,910,133,96,804
366,918,277,84,308,945,916,421,144,886,460,491,178,610,133,399,138,124,425,401
712,906,340,616,159,156,882,160,744,818,882,717,922,763,508,215,108,898,73,114
520,903,515,613,913,525,570,157,542,761,727,536,877,703,350,585,567,516,727,883
219,239,710,736,941,605,71,864,449,217,892,836,92,706,295,486,428,193,912,934
236,702,55,559,673,166,72,878,798,546,535,84,766,559,312,510,74,301,790,391
302,682,726,711,565,157,136,576,943,221,941,214,681,732,458,539,616,939,904,677
144,494,883,211,752,643,839,537,722,881,76,520,493,224,565,659,578,216,679,655
853,862,829,424,586,376,646,815,917,423,924,728,657,887,886,396,128,499,886,167
860,218,530,278,504,650,215,826,64,377,742,376,72,372,802,353,994,166,311,69
377,824,422,372,919,397,195,812,887,809,469,724,65,314,716,703,706,681,837,423
809,267,600,395,857,406,366,150,615,531,541,423,608,84,413,799,609,834,533,712
820,814,222,904,405,74,81,749,90,683,61,354,419,552,910,496,565,651,823,371
873,831,550,312,418,126,147,293,374,243,574,225,876,552,318,359,306,229,763,210
196,494,73,806,648,121,839,449,187,554,424,928,250,797,227,189,127,125,173,424";

    }
}
