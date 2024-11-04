import os
import requests
from bs4 import BeautifulSoup

# Function to get emoji name from emojipedia based on the file name
def get_emoji_name(file_name):
    # Replace spaces with '+' for URL formatting
    query = file_name.replace(' ', '+')
    url = f"https://emojipedia.org/search/?q={query}"
    
    # Make a request to emojipedia with the search query
    response = requests.get(url)
    
    if response.status_code == 200:
        soup = BeautifulSoup(response.content, 'html.parser')
        
        # Find the first <span> element with the specified class
        span_element = soup.find('span', class_='EmojisList_emojis-list-item-title__kiHdi')
        
        if span_element:
            return span_element.get_text().strip()
        else:
            print(f"No emoji title found for {file_name}.")
            return None
    else:
        print(f"Failed to retrieve data for {file_name}. Status code: {response.status_code}")
        return None

# Loop over all .png files in the current directory
def rename_files_in_directory():
    for file in os.listdir():
        if file.endswith(".png"):
            file_name = os.path.splitext(file)[0]
            print(f"Processing file: {file_name}")
            
            # Get the emoji name from emojipedia
            new_name = get_emoji_name(file_name)
            
            if new_name:
                new_file_name = new_name + ".png"
                
                # Rename the file
                os.rename(file, new_file_name)
                print(f"Renamed {file} to {new_file_name}")
            else:
                print(f"Skipping rename for {file}")

# Run the renaming process
rename_files_in_directory()