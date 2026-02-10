import fs from 'node:fs';

class TxtParser {
	readonly filePath: string;
	readonly rowDelimiter = '~';
	readonly fieldDelimiter = '|';
	readonly endDelimiter = '#EOF#';

	constructor(filePath: string) {
		this.filePath = filePath;
	}

	async txtToJson() {
		console.log("I'm parsing here!");
		const companiesArr: txtType[] = [];

		const textContent = fs.readFileSync(this.filePath, 'utf8').trim().replace(this.endDelimiter, '');

		const rows = textContent.split(this.rowDelimiter);

		rows.forEach((row) => {
			const currRow = row.split(this.fieldDelimiter);

			const txtOutputJson: txtType = {
				cvr: currRow[0],
				full_name: currRow[1],
				street: currRow[2],
				number: currRow[3],
				postal_code: currRow[4],
				city: currRow[5],
				email: currRow[6],
			};

			companiesArr.push(txtOutputJson);
		});

		console.log(companiesArr);
	}
}

export { TxtParser };
