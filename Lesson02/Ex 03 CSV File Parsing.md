### CSV File parsing
Write a CSV parser API in the programming language of your choice, with the following assumptions:
- The CSV file includes a header row
- The API receives the path to the csv file as a parameter
- The API returns the file's content in JSON format, so that each column name constitutes a key

*Example*

CSV file:
```csv
first_name,last_name,age
Alexander M.,Overgaard,38
Celina J.,Iversen,55
```
API output:
```json
[
  {
    "first_name": "Alexander M.",
    "last_name": "Overgaard",
    "age": "38"
  },
  {
    "first_name": "Celina J.",
    "last_name": "Iversen",
    "age": "55"
  }
]
```

Use [this fake Danish companies CSV file](https://github.com/arturomorarioja-ek/SD_System_Integration_F2026/blob/main/Lesson02/danish_companies.csv) for testing.
