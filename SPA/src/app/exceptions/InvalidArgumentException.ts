export class InvalidArgumentException extends Error {

  constructor(args: string[]) {
    const message: string = `Arguments ${args.join(", ")} are invalid.`;

    super(message);

    this.name = "InvalidArgumentException";
  }

}
