package com.TheDisappointedProgrammer.hello;

import org.springframework.beans.factory.annotation.Autowired;

import java.io.IOException;
import java.io.Reader;

public abstract class McCanceTable<T> {
    @Autowired
    protected McCanceReader mcCanceReader;

    void processCSVData(Reader rdr) throws IOException {
        mcCanceReader.processMcCanceCSVFile(rdr);
    }

    /**
     * processMcCanceCSVFile must be processed before this is called
     * @return some sort of collection of data from the CSV file
     */
    abstract T getData();
}
