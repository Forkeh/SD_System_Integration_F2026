import express from 'express';
import { CsvParser } from './csv-parser';
import { TxtParser } from './text-parser';

const app = express();
const PORT = 3000;

// ----- Middleware -----
app.use(express.json());

// ----- Routes -----
app.get('/', (req, res) => {
	res.json({ message: 'Welcome to the Parser Exercise API!' });
});

// Text Parser
app.get('/api/companies/txt', async (req, res) => {
	try {
		const textParser = new TxtParser('../danish_companies.txt');
		const companies = await textParser.txtToJson();
		console.log(companies);

		res.json(companies);
	} catch (error) {
		res.status(500).json({ error: 'Failed to parse TXT file' });
	}
});

// CSV Parser
app.get('/api/companies/csv', async (req, res) => {
	try {
		const csvParser = new CsvParser('../danish_companies.csv');
		const companies = await csvParser.csvToJson();
		res.json(companies);
	} catch (error) {
		res.status(500).json({ error: 'Failed to parse CSV file' });
	}
});

// Get Repos by Username
app.get('/api/github/:username/repos', async (req, res) => {
	const { username } = req.params;

	try {
		const response = await fetch(`https://api.github.com/users/${username}/repos`);

		const result = await response.json();

		res.json(result);
	} catch (error) {
		res.status(500).json({ error: 'Failed to get user repositories' });
	}
});

// Get Files by Repo
app.get('/api/github/:username/:repo/files', async (req, res) => {
	const { username, repo } = req.params;

	try {
		const response = await fetch(`https://api.github.com/repos/${username}/${repo}/contents`);

		const result = await response.json();

		res.json(result);
	} catch (error) {
		res.status(500).json({ error: 'Failed to get user repositories' });
	}
});

// Display raw file contents
app.get('/api/github/:username/:repo/:branch/raw/:path(*)', async (req, res) => {
	const { username, repo, branch, path } = req.params;

	try {
		const response = await fetch(`https://raw.githubusercontent.com/${username}/${repo}/${branch}/${path}`);

		const result = await response.text();

		res.send(result);
	} catch (error) {
		res.status(500).json({ error: 'Failed to get file contents' });
	}
});

// Start server
app.listen(PORT, () => {
	console.log(`Server is running on http://localhost:${PORT}`);
});

// ----- Original Parser Exercises (commented out) -----
// ----- Exercise 01 -----
// const textParser = new TxtParser('../danish_companies.txt');
// const textTojsonCompanies = textParser.txtToJson();
// console.log(textTojsonCompanies);

// ----- Exercise 02 -----
// const csvParserCompanies = new CsvParser('../danish_companies.csv');
// const csvToJsonCompanies = await csvParserCompanies.csvToJson();
// console.log(csvToJsonCompanies);
