- [x] A pricing table has at least one tier
- [x] A pricing table has a set of tier
- [x] Pricing Table covers 24h
- [x] Pricing tiers ordered by hour limit
- [x] Max Price eq to sum of tiers if not defined
- [x] Max Price eq to provided value
- [x] Max Price < full day though table

----

- [x] Improve exception-based tests
- [x] Hour limit should be between 1 and 24
- [x] The price can't be negative

----

- [x] Fail if request is null
- [x] Return bool as true if succeed
- [x] Invoke storage only one
- [x] Request is mapped correctly to storage
- [x] External dependecy is invoked to store pricing and that it stored the input

----

- [x] Throw exception if missing connection
- [x] Insert if not exists
- [x] Replace if exists
- [x] Replace keeps new values
- [x] Return true if succeed

----

- [x] 500 if unknown error
- [x] 200 if success
- [x] 400 if returns false

----

- [ ] ticker for 30min when 1h has the cost of 2.5
- [ ] Entry and Exit time
- [ ] Get Price table from storage