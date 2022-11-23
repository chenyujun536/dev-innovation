import turtle
import random
import math
import multiprocessing as mp
import sys
from stopwatch import *

t = turtle
length = 200
dot_per_group = 1000
dot_group_count = 1000
radius = length/2
center_x = length/2
center_y = length/2

def draw_square():
    t.forward(length)
    t.left(90)
    t.forward(length)
    t.left(90)
    t.forward(length)
    t.left(90)
    t.forward(length)
    t.left(90)

def calculate_pi_and_draw():
    tw = stopwatch()
    draw_square()
    t.penup()
    t.speed(0)

    dots_in_circle = 0

    for i in range(dot_per_group):
        x = random.uniform(0,length)
        y = random.uniform(0,length)

        distance_to_center = math.sqrt((x-center_x)**2 + (y-center_y)**2)

        if distance_to_center < radius:
            dots_in_circle = dots_in_circle + 1
            t.pencolor("red")
        else:
            t.pencolor("blue")
        t.goto(x,y)
        t.dot()
        if i%100==0:
            print(i, "done out of ", dot_per_group)
    
    pi = 4 * dots_in_circle / dot_per_group
    print("pi is ", pi, ". use time ", tw.stop())


def random_dot_once(group):
    sum = 0
    for i in range(dot_per_group):
        x = random.uniform(0,length)
        y = random.uniform(0,length)

        distance_to_center = math.sqrt((x-center_x)**2 + (y-center_y)**2)

        if distance_to_center < radius:
            sum = sum + 1
    return sum

def calculate_pi_and_parallel():
    tw = stopwatch()
    pool = mp.Pool(mp.cpu_count())

    results = pool.map(random_dot_once, [group for group in range(dot_group_count)])

    pool.close()

    pi = 4 * sum(results) / (dot_group_count * dot_per_group)
    
    print("Calculate", (dot_group_count * dot_per_group), " dots. PI is ", pi, ". Use time ", tw.stop())


def main():

    if len(sys.argv) > 1 and sys.argv[1] == "d":
        calculate_pi_and_draw()
    else:
        calculate_pi_and_parallel()

    

if __name__ == '__main__':
    main()