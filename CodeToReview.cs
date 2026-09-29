using System;
// [BLOCKER] Typo here: `Collegctions` -> `Collections`. This currently prevents compilation.
using System.Collegctions.Generic;
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    public class People
    {
        // [MINOR] `Under16` is a little misleading since this is exactly 15 years ago.
        // Consider a clearer name or calculate the default DOB at construction time.
        private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);

        public string Name { get; private set; }
        public DateTimeOffset DOB { get; private set; }

        public People(string name) : this(name, Under16.Date) { }

        public People(string name, DateTime dob) {
            Name = name;
            DOB = dob;
        }
    }

    public class BirthingUnit
    {
        private List<People> _people;

        public BirthingUnit()
        {
            _people = new List<People>();
        }

        public List<People> GetPeople(int i)
        {
            for (int j = 0; j < i; j++)
            {
                try
                {
                    string name = string.Empty;

                    // [MINOR] Create Random once outside the loop (or use Random.Shared).
                    var random = new Random();

                    // [BUG] Next's upper bound is exclusive, so this always returns 0.
                    // As a result, Betty is never generated.
                    if (random.Next(0, 1) == 0) {
                        name = "Bob";
                    }
                    else {
                        name = "Betty";
                    }

                    // [BUG] 356 is not a calendar year. This can produce incorrect DOBs.
                    // Prefer AddYears() for age-based calculations.
                    _people.Add(new People(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0))));
                }
                catch (Exception e)
                {
                    // [BUG] The original exception is lost here. Unless we can recover,
                    // I'd remove this catch; otherwise preserve `e` as the inner exception.
                    throw new Exception("Something failed in user creation");
                }
            }

            // [DESIGN] This returns the internal mutable list. Consider IReadOnlyList<People>
            // so callers cannot modify BirthingUnit's state directly.
            //
            // [DESIGN] Also, this method adds `i` items but returns the entire accumulated
            // list. Is that the intended contract? A second call will return more items
            // than requested.
            return _people;
        }

        private IEnumerable<People> GetBobs(bool olderThan30)
        {
            // [BUG] This condition is reversed for "older than 30".
            // People older than 30 have DOB <= the 30-year cutoff.
            //
            // [BUG] Same 356-day approximation as above; use AddYears(-30).
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob");
        }

        public string GetMarried(People p, string lastName)
        {
            // [MINOR] Consider validating p/lastName at the public API boundary.
            if (lastName.Contains("test"))
                return p.Name;

            // [BUG] Substring's result is ignored, so the name is never truncated.
            // Also, the length check doesn't include the space added below.
            if ((p.Name.Length + lastName).Length > 255)
            {
                (p.Name + " " + lastName).Substring(0, 255);
            }

            return p.Name + " " + lastName;
        }
    }
}

/*
PR SUMMARY

Request changes.

Main issues to address before merge:
- Fix the `Collections` namespace typo.
- Fix `Random.Next(0, 1)`; Betty can never currently be generated.
- Use calendar years instead of 356-day approximations.
- Fix the reversed `olderThan30` comparison.
- Fix the ignored `Substring()` result.
- Clarify `GetPeople()` behaviour and avoid exposing the internal List.

A couple of smaller cleanup items:
- Reuse Random / use Random.Shared.
- Preserve the original exception if wrapping is really needed.
- Add argument validation for public methods.
- Consider clearer names such as `Person`, `DateOfBirth` and `count`.

I'd also add unit tests around the age-30 boundary, both generated names,
the 255-character limit, and repeated calls to GetPeople().

Once the above correctness issues are fixed, I'd be happy to re-review.
*/
