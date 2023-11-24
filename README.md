# Instructions for the assignment

1. Clone this repository on your machine.
1. Use your IDE of choice to complete the assignment.
1. When you are finished with the solution and have pushed it to the repo, [you can submit the assignment here](https://app.snapcode.review/submission_links/c7ec9ca6-9a0f-44f4-9d7b-e9e5583fce36).
1. There is no time limit for this task

## Tips on what we’re looking for

We value simplicity as an architectural virtue and as a development practice. Solutions should reflect the difficulty of the assigned task, and shouldn’t be overly complex. We prefer simple, well tested solutions over clever solutions. 

### DO
* ✅ Include unit tests
* ✅ Test both any client and logic
* ✅ Update the README.md with any relevant information and/or tradeoffs you would like to highlight

### DO NOT

* ❌ Submit a web, desktop, command line or any other kind of app
* ❌ Add unnecessary layers of abstraction
* ❌ Add unnecessary patterns/ architectural features that aren’t called for

# Begin the task

Create a shopping cart ***package*** that facilitates 2 basic capabilities.

1. Add a product to the cart
   1. Specifying the product name and quantity
   2. Use the product name to discover the price from the [Product API](#product-api) specified below
   3. Cart state (totals, etc.) must be available

2. Calculate the state:
   1. Cart subtotal (sum of price for all items)
   2. Tax payable (charged at 12.5% on the subtotal)
   3. Total payable (subtotal + tax)
   4. Totals should be rounded up where required

## Product API

Base URL: `https://equalexperts.github.io/`

View Product: `GET /backend-take-home-test-data/{product}.json`

Available products
* `cheerios`
* `cornflakes`
* `frosties`
* `shreddies`
* `weetabix`

## Example
The below is a sample with the correct values you can use to confirm your calculations

### Inputs
* Add 1 × cornflakes @ 2.52 each
* Add another 1 x cornflakes @2.52 each
* Add 1 × weetabix @ 9.98 each
  
### Results  
* Cart contains 2 x cornflakes
* Cart contains 1 x weetabix
* Subtotal = 15.02
* Tax = 1.88
* Total = 16.90
