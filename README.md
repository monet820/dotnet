# test-template
Reference for how to test a project.

# Why test?
Make your code make sense.
Make refactoring easier, ( maybe when upgrading an old solution? )

## Integration tests
## Unit tests
## End to End tests

# Unit testing
## What to test
1. Class
2. Method
3. One line of code

## What not to test
No Input/Output
We assume they work, and mock a reply.

1. No database calls
2. No network calls
3. No file-system calls

# Component testing
Unit testing, but bigger ( more units )

# Integration testing
Tests the integration
We don't assume anything! ( no mocking )
1. Dependencies
   2. Databases
   3. File system
   4. Network
5. No mocking

# Other tests
1. End to end testing
2. performance testing
3. smoke testing

# Testing pyramid ( most expensive, and slower )
1. End to End testing
2. Integration testing (happy and unhappy paths)
3. Unit testing (niche cases)

