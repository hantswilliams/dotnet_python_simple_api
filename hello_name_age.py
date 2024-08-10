import sys
import pandas as pd
import numpy as np

def process_info(name, age):

    ## create a empty dataframe for testing purposes
    df = pd.DataFrame()

    ## using some numpy functions for testing purposes
    df['random'] = np.random.randn(10)

    try:
        age = int(age)
        return f"Hello {name}, you are {age} years old. And this is a random number: {df['random'].mean()}"
    except ValueError:
        return "Error: Age must be an integer."

if __name__ == "__main__":
    # Expecting two arguments: name and age
    if len(sys.argv) != 3:
        print("Error: Please provide a name and an age.")
    else:
        name = sys.argv[1]
        age = sys.argv[2]
        print(process_info(name, age))
