import tkinter as tk
from tkinter import filedialog
import zipfile
import os

def compress_folder(compression_method):
    folder_path = filedialog.askdirectory(title="Select the folder to compress")
    if folder_path:
        zip_name = filedialog.asksaveasfilename(defaultextension=".zip", filetypes=[("ZIP files", "*.zip")], title="Save as")
        if zip_name:
            compression = zipfile.ZIP_DEFLATED if compression_method == "Default" else zipfile.ZIP_LZMA
            with zipfile.ZipFile(zip_name, 'w', compression) as zipf:
                for root, _, files in os.walk(folder_path):
                    for file in files:
                        full_path = os.path.join(root, file)
                        zipf.write(full_path, os.path.relpath(full_path, folder_path))
            confirmation_message.config(text="Folder compressed successfully using " + compression_method + " method.")

# Create the main window
window = tk.Tk()
window.title("Folder Compressor")
window.resizable(width=True, height=True)
window.geometry("600x400")

# Function to handle user's choice of compression method
def choose_compression():
    compression_choice = compression_var.get()
    compress_folder(compression_choice)

# Radio buttons for compression method selection
compression_var = tk.StringVar()
compression_var.set("Default")

default_radio = tk.Radiobutton(window, text="Default", variable=compression_var, value="Default")
default_radio.pack()

lzma_radio = tk.Radiobutton(window, text="LZMA", variable=compression_var, value="LZMA")
lzma_radio.pack()

# Button to compress the folder using the selected method
compress_button = tk.Button(window, text="Compress Folder", command=choose_compression)
compress_button.pack(pady=10)

# Label to display confirmation message
confirmation_message = tk.Label(window, text="")
confirmation_message.pack()

# Run the graphical interface
window.mainloop()