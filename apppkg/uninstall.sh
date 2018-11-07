#!/bin/bash

sfctl application delete --application-id census2tierapp
sfctl application unprovision --application-type-name census2tierappType --application-type-version 1.0.0
sfctl store delete --content-path census2tierapp
