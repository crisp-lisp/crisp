cmake_minimum_required(VERSION 3.2)
project(crisp)

set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -Werror")

set(SOURCE_FILES main.c)
add_executable(crisp ${SOURCE_FILES})