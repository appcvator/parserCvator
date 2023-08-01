
import requests

def get_job_details(url):
    # Send a GET request to the URL
    response = requests.get(url)
    
    # Check the status code of the response, raise an exception if the request failed
    response.raise_for_status()

    # Return the content of the response as a string
    return response.text

# Test the function
# print(get_job_details('https://test.com'))