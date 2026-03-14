export type Task = {
	title: string;
	completed: boolean;
};

export const tasks: Task[] = [
	{ title: 'Learn GraphQL', completed: false },
	{ title: 'Build a GraphQL API', completed: true },
	{ title: 'Test the API', completed: false },
	{ title: 'Keep learning', completed: false },
];
