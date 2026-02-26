### XML File Parsing
Write an XML parser API in the programming language of your choice, with the following assumptions:
- The API receives the path to the XML file as a parameter
- The API returns the file's content in JSON format
- XML attributes will be converted to JSON keys

*Example*

XML file:
```xml
<?xml version="1.0" encoding="utf-8"?>
<companies>
  <company>
    <cvr>78836568</cvr>
    <name>Dansk Consult ApS &amp; Co.</name>
    <street>Parkvej</street>
    <number>123</number>
    <postal_code>6700</postal_code>
    <city>Esbjerg</city>
    <email>info0@dansk.dk</email>
  </company>
  <company>
    <cvr>67982862</cvr>
    <name>Viking IT IVS</name>
    <street>Torvegade</street>
    <number>132</number>
    <postal_code>3400</postal_code>
    <city>Hillerød</city>
    <email>info1@viking.dk</email>
  </company>
</companies>
```
API output:
```json
{
    "companies": {
        "company": [
            {
                "city": "Esbjerg",
                "cvr": "78836568",
                "email": "info0@dansk.dk",
                "name": "Dansk Consult ApS & Co.",
                "number": "123",
                "postal_code": "6700",
                "street": "Parkvej"
            },
            {
                "city": "Hillerød",
                "cvr": "67982862",
                "email": "info1@viking.dk",
                "name": "Viking IT IVS",
                "number": "132",
                "postal_code": "3400",
                "street": "Torvegade"
            }
        ]
    }
}
```

Use [this fake Danish companies XML file](https://github.com/arturomorarioja-ek/SD_System_Integration_F26_Materials/blob/main/File%20Formats/Exercises/danish_companies.xml) for testing.

### Solution
[Python](https://github.com/arturomorarioja/py_xml_parser_api)
