### YAML File parsing
Write a YAML parser API in the programming language of your choice, with the following assumptions:
- The API receives the path to the YAML file as a parameter
- The API returns the file's content in JSON format

*Example*

YAML file:
```yaml
- cvr: '78836568'
  name: Dansk Consult ApS & Co.
  street: Parkvej
  number: '123'
  postal_code: '6700'
  city: Esbjerg
  email: info0@dansk.dk
- cvr: '67982862'
  name: Viking IT IVS
  street: Torvegade
  number: '132'
  postal_code: '3400'
  city: Hillerød
  email: info1@viking.dk
```
API output:
```json
[
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
```

Use [this fake Danish companies YAML file](https://github.com/arturomorarioja-ek/SD_System_Integration_F2026/blob/main/Lesson02/danish_companies.yml) for testing.
