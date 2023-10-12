using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Models.Entities;

[ApiController]
[Route("api/[controller]")]
public class NotesController : Controller
{
    private readonly NotesDbContext _notesDbContext;

    public NotesController(NotesDbContext notesDbContext)
    {
        _notesDbContext = notesDbContext;
    }

    // Получаем заметки из БД
    [HttpGet]
    public async Task<IActionResult> GetAllNotes()
    {
        return Ok(await _notesDbContext.Notes
            .AsNoTracking()
            .ToListAsync());
    }

    // Получаем заметку по Id
    [HttpGet]
    [Route("{id:Guid}")]
    [ActionName("GetNoteById")]
    public async Task<IActionResult> GetNoteById([FromRoute] Guid id)
    {
        var note = await _notesDbContext.Notes.FindAsync(id);
        if (note is null)
        {
            return NotFound();
        }
        
        return Ok(note);
    }

    // Добавление заметки
    [HttpPost]
    public async Task<IActionResult> AddNote(Note note)
    {
        note.Id = Guid.NewGuid();
        await _notesDbContext.Notes.AddAsync(note);
        await _notesDbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNoteById), new {id = note.Id}, note);
    }

    // Обновление заметки
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateNote([FromRoute] Guid id, [FromBody] Note updateNote)
    {
       var existingNote = await _notesDbContext.Notes.FindAsync(id); 

       if (existingNote is null)
       {
        return NotFound();
       }

       existingNote.Title = updateNote.Title;
       existingNote.Description = updateNote.Description;
       existingNote.IsVisible = updateNote.IsVisible;

       await _notesDbContext.SaveChangesAsync();

       return Ok(existingNote);

    }

    // Удаление заметки
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
    {
       var existingNote = await _notesDbContext.Notes.FindAsync(id); 

       if (existingNote is null)
       {
        return NotFound();
       }

       _notesDbContext.Notes.Remove(existingNote);
       await _notesDbContext.SaveChangesAsync();

       return Ok();
    }


}