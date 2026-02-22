import fs from 'node:fs';
import { XMLParser, XMLBuilder, XMLValidator } from 'fast-xml-parser';

class XmlParser {
	private readonly filePath: string;

	constructor(filePath: string) {
		this.filePath = filePath;
	}

	async xmlToJson() {
		try {
			const xmlData = fs.readFileSync(this.filePath, 'utf8');

			const validation = XMLValidator.validate(xmlData);

			if (validation !== true) {
				throw new Error(`Invalid XML: ${validation.err.msg} (line ${validation.err.line})`);
			}

			const options = {
				ignoreAttributes: false,
				attributeNamePrefix: '@_',
				ignoreDeclaration: true,
			};

			const XmlParser = new XMLParser(options);
			const jsonObj = XmlParser.parse(xmlData);

			return jsonObj;
		} catch (error) {
			console.error('Failed to parse XML file:', error instanceof Error ? error.message : String(error));

			throw error;
		}
	}
}

export { XmlParser };
