using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_X_O.Services;
using Web_X_O.Models;

public class IndexModel : PageModel
{
    private readonly GameManager manager;

    public IndexModel(GameManager manager)
    {
        this.manager = manager;
    }

    [BindProperty]
    public int? Size { get; set; }

    [BindProperty]
    public int? WinLength { get; set; }

    public IActionResult OnPost()
    {
        if (!Size.HasValue)
            ModelState.AddModelError(nameof(Size), "Укажите размер поля.");
        else if (Size.Value < Game.MinSize || Size.Value > Game.MaxSize)
            ModelState.AddModelError(nameof(Size),
                $"Размер поля должен быть от {Game.MinSize} до {Game.MaxSize}.");

        if (!WinLength.HasValue)
            ModelState.AddModelError(nameof(WinLength), "Укажите длину победной линии.");
        else if (WinLength.Value < Game.MinSize)
            ModelState.AddModelError(nameof(WinLength),
                $"Длина победной линии должна быть не менее {Game.MinSize}.");
        else if (Size.HasValue && WinLength.Value > Size.Value)
            ModelState.AddModelError(nameof(WinLength),
                "Длина победной линии не может быть больше размера поля.");

        if (!ModelState.IsValid)
            return Page();

        var id = Guid.NewGuid().ToString();
        manager.CreateGame(id, Size!.Value, WinLength!.Value);
        return Redirect($"/Game/{id}");
    }

}
