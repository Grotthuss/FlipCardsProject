import React from 'react';
import './App.css';
import FlipCard from './FlipCard';
import CardSetSelection from './CardSetSelection';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<CardSetSelection />} />
                <Route path="/card-set/:title" element={<FlipCard />} />
            </Routes>
        </Router>
    );
}

export default App;
