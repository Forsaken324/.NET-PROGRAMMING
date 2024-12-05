using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Utilities;

public class PaginatedList<T> : List<T>
{
    public int PageIndex {get; private set;}
    public int TotalPages {get;  private set;}

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        
        /* calculates the total number of pages using the size of each page, i.e how many items a page can contain 
        and the total number of items. So think of it like this; if we have 50 items and one page can only contain 2 items
        the total number of pages we will have is 50/2 which is 25. */
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        // adds the given items to the paginated list instance
        this.AddRange(items);

    }

    /* calculates if there is a page to go back to by checking if the index of the page is greater than 1 i.e the page number*/
    public bool HasPreviousPage => PageIndex > 1;
    /* calculates if there is another page to move forward to by checking if the index of the page is less than the total number of pages*/
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}