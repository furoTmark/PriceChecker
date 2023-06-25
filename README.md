# PriceChecker

Test app for Faptic Technologies

In order to start, navigate to the root folder and perform the following command:  
`docker-compose up`

App should start and be available at:  
`http://localhost:8008/swagger/index.html`

### Example requests

Requests the aggregated bitcoin price at a specific time point with hour accuracy:
http://localhost:8008/BitcoinPrice/v1/aggregated/2023-06-23/0

Fetches the persisted bitcoin prices from the datastore during a user-specified time range:
http://localhost:8008/BitcoinPrice/v1/persisted?start=2023-06-23T00%3A00&end=2023-06-23T23%3A30