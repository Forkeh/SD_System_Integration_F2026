### Txt File Parsing
Write a text file parsing API in the programming language of your choice, with the following assumptions:
- The text file contains information about fake Danish companies
- Each row is finished by the character `~`, except the last one, finished by the sequence `#EOF#`
- Each field is finished by the character `|`
- There is no header row
- The fields for each row contain the following values:
  - CVR
  - Company name
  - Street
  - Number
  - Postal Code
  - Town
  - Email address
- The API receives the path to the text file as a parameter
- The API returns the file's content in JSON format
  
*Example*

Text file:

```
21224947|Viking Foods ApS & Co.|Søndergade|60|1000|København|info0@viking.dk~80444806|Øresund Byg ApS & Co.|Hovedgaden|108|4800|Nykøbing Falster|info1@øresund.dk#EOF#
```

API output:

```json
[
    {
        "cvr": "21224947",
        "full_name": "Viking Foods ApS & Co.",
        "street": "Søndergade",
        "number": "60",
        "postal_code": "1000",
        "city": "København",
        "email": "info0@viking.dk"
    },
    {
        "cvr": "80444806",
        "full_name": "Øresund Byg ApS & Co.",
        "street": "Hovedgaden",
        "number": "108",
        "postal_code": "4800",
        "city": "Nykøbing Falster",
        "email": "info1@øresund.dk"
    }
]
```
 
Use the file [`danish_companies.txt`](https://github.com/arturomorarioja-ek/SD_System_Integration_F2026/blob/main/Lesson02/danish_companies.txt) for testing.
