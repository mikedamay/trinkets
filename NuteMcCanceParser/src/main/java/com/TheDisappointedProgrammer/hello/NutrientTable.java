package com.TheDisappointedProgrammer.hello;

import org.springframework.stereotype.Component;

import java.util.stream.Stream;

@Component
public class NutrientTable extends McCanceBaseTable<Stream<McNutrient>> {
    @Override
    Stream<McNutrient> getData() {
        return mapToStream(mcCanceReader);
    }
    Stream<McNutrient> mapToStream(McCanceReader mcCanceReader) {
        return mcCanceReader.getNutrients().entrySet().stream().map(es -> new McNutrient(es.getKey(), es.getValue()));
    }
}
