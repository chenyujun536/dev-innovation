import time

class stopwatch:
    def __init__(self):
        self.start_time = time.time()
    
    def start(self):
        self.start_time = time.time()
    
    def stop(self):
        self.stop_time = time.time()
        time_lapsed = self.stop_time - self.start_time
        mins = time_lapsed // 60
        sec = time_lapsed % 60
        hours = mins // 60
        mins = mins % 60
        return("Time Lapsed = {0}:{1}:{2}".format(int(hours),int(mins),sec))