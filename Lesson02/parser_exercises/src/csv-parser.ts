import Papa from 'papaparse';
import fs from 'node:fs';

class CsvParser {
	private readonly filePath: string;

	constructor(filePath: string) {
		this.filePath = filePath;
	}

	async csvToJson() {
		try {
			const csvFile = fs.readFileSync(this.filePath, 'utf-8');

			const data = Papa.parse(csvFile, {
				header: true,
				skipEmptyLines: true,
			});

			// console.log('Data:', data.data);
			// console.log('Errors:', data.errors);
			// console.log('Meta:', data.meta);

			return data;
		} catch (error) {
			console.error('Failed to parse CSV file:', error instanceof Error ? error.message : String(error));

			throw error;
		}
	}
}

export { CsvParser };
