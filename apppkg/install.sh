#!/bin/bash

sfctl application upload --path census2tierapp --show-progress
sfctl application provision --application-type-build-path census2tierapp
sfctl application create --app-name fabric:/census2tierapp --app-type census2tierappType --app-version 1.0.0
