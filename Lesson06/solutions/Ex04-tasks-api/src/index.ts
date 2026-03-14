import { ApolloServer } from '@apollo/server';
import { startStandaloneServer } from '@apollo/server/standalone';
import { Task, tasks } from './data/tasks.js';

const typeDefs = `#graphql

  type Task {
    title: String!
    completed: Boolean!
  }

  type Query {
    tasks: [Task]
    task(title: String!): Task
  }

  type Mutation {
    addTask(title: String!, completed: Boolean!): Task!
	updateTask(title: String!, completed: Boolean!): Task!
	deleteTask(title: String!): Task!
  }
`;

const resolvers = {
	Query: {
		tasks: () => tasks,
		task: (_, args: { title: string }) => tasks.find((t) => t.title === args.title),
	},

	Mutation: {
		addTask: (_, args: { title: string; completed: boolean }) => {
			const newTask: Task = { title: args.title, completed: args.completed };
			tasks.push(newTask);
			return newTask;
		},
		updateTask: (_, args: { title: string; completed: boolean }) => {
			const currIndex = tasks.findIndex((t) => t.title === args.title);

			if (currIndex === -1) throw new Error(`Task "${args.title}" not found`);

			const updatedTask: Task = { title: args.title, completed: args.completed };

			tasks.splice(currIndex, 1, updatedTask);

			return updatedTask;
		},
		deleteTask: (_, args: { title: string }) => {
			const currIndex = tasks.findIndex((t) => t.title === args.title);

			if (currIndex === -1) throw new Error(`Task "${args.title}" not found`);

			return tasks.splice(currIndex, 1)[0];
		},
	},
};

const server = new ApolloServer({
	typeDefs,
	resolvers,
});

const { url } = await startStandaloneServer(server, {
	listen: { port: 4000 },
});

console.log(`🚀  Server ready at: ${url}`);
