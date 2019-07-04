namespace TestOkur.Infrastructure.Extensions
{
	using System;
	using System.Collections.Generic;

	public static class EnumerationExtensions
	{
		/// <summary>
		/// Enumerate a range performing an action on each item.
		/// </summary>
		/// <typeparam name="T">The type of the range to enumerate.</typeparam>
		/// <param name="collection">The rage to enumerate.</param>
		/// <param name="doThis">The action to perform on each action.</param>
		public static void Each<T>(this IEnumerable<T> collection, Action<T> doThis)
		{
			foreach (var item in collection)
			{
				doThis(item);
			}
		}
	}
}
