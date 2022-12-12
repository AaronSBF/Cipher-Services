using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CipherServices.Services;
using CipherServices.Data;
using CipherServices.Models;

namespace CipherServices.Pages
{
  public class IndexModel : PageModel
  {
    public Dictionary<string, string> Secrets { get; set; }
    private readonly IDecrypter _decrypter;
    private readonly IEncrypter _encrypter;
    private readonly MessageContext _context;

    [BindProperty]
    public Message NewMessage { get; set; }

    public IndexModel()
    {

    }

    public async Task<IActionResult> OnGet()
    {
     await LoadSecretsAsync(_decrypter, _context);
      return Page();
    }

public async Task<IActionResult> OnPostAsync(){
  if(ModelState.IsValid){
    string clean = NewMessage.Text.Trim().ToLower();
    string encryptedText = _encrypter.Encrypt(clean);
    Message m = new Message{Text= encryptedText};
    _context.Messages.Add(m);
    await _context.SaveChangesAsync();

    return RedirectToPage("/Index");
  }else{
   await LoadSecretsAsync(_decrypter, _context);
    return Page();
  }
}

    private async Task LoadSecretsAsync(IDecrypter decrypter, MessageContext context)
    {
      Secrets = new Dictionary<string, string>();
      var messages = await context.Messages.ToListAsync();

      foreach (Message m in messages)
      {
        Secrets.TryAdd(m.Text, decrypter.Decrypt(m.Text));
      }
    }
  }
}
