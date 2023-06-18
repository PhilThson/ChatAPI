using System;
namespace ChatAPI.Infrastructure.Exceptions
{
	public class ValidationException : Exception
	{
		public ValidationException(string message) : base(message)
		{
		}

		public ValidationException() : base("Invalid input data")
		{
		}
	}

	public class NotFoundException : Exception
	{
		public NotFoundException(string msg) : base(msg)
		{
		}

		public NotFoundException() : base("No record in database")
		{
		}
	}

	public class AuthenticationException : Exception
	{
		public AuthenticationException(string msg) : base(msg)
		{
		}

		public AuthenticationException() : base("Unauthenticated user")
		{
		}
	}

	public class UnknownUserException : Exception
	{
		public UnknownUserException(string msg) : base(msg)
		{
		}

		public UnknownUserException() : base("Request doesn't contain user data")
		{
		}
	}
}

