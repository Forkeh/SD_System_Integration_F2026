import { TxtParser } from './text-parser';

// ----- Exercise 01 -----
const textParser = new TxtParser('../danish_companies.txt');

const jsonCompanies = textParser.txtToJson();

console.log(jsonCompanies);

// ----- Exercise 02 -----
