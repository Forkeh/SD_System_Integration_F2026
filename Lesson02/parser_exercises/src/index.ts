import { TxtParser } from './parsers';

console.log('Hello from TypeScript!');

const textParser = new TxtParser('../danish_companies.txt');

textParser.txtToJson();
